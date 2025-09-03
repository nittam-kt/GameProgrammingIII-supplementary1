using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Character : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float avoidDistance;
    [SerializeField] float rotateSpeed;

    public CharacterManager manager;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
    }

    void Update()
    {
        rb.linearVelocity = transform.forward * speed;

        manager.GetGrid(transform.localPosition, out int gridX, out int gridY);
//        foreach (var c in manager.characterList)
        foreach (var c in manager.characterGirds[gridX][gridY])
        {
            if (c == this) continue;

            var len = Vector3.Distance(c.transform.position, transform.position);

            // 避けるべき距離より近ければ
            if (c != this && len < avoidDistance)
            {
                // 相手方向から離れる方向へ回転
                leaveRotate(c.transform.position - transform.position);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach(var contact in collision.contacts)
        {
            if (contact.otherCollider.gameObject.layer == 6) // レイヤーが壁
            {
                var dir = (transform.forward - contact.normal) * 0.5f;
                leaveRotate(dir); // 法線の逆方向と前方の中間方向から離れる
            }
        }
    }

    // 指定した方向から離れる方向に回転する
    void leaveRotate(Vector3 dir)
    {
        Vector2 toFlat = new Vector2(dir.x, dir.z).normalized;
        Vector2 forwardFlat = new Vector2(transform.forward.x, transform.forward.z).normalized;

        // 前方向成分（コサイン値）
        float dot = Vector2.Dot(forwardFlat, toFlat);
        // 60度以内かどうか（cos60°= 0.5）
        bool isWithin60 = dot > 0.5f;

        // 右か左か（右ならcross.z > 0, 左ならcross.z < 0）
        float crossZ = Vector3.Cross(forwardFlat, toFlat).z;
        bool isRight60 = isWithin60 && crossZ < 0f;
        bool isLeft60 = isWithin60 && crossZ > 0f;

        if (isRight60)
        {
            // 右60度以内なら左回転
            // 右回転
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, -rotateSpeed * Time.deltaTime, 0));
        }
        else if (isLeft60)
        {
            // 左60度以内なら右回転
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotateSpeed * Time.deltaTime, 0));
        }
    }
}

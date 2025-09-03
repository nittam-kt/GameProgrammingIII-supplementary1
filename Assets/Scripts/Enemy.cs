using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float senceRange = 5f;
    [SerializeField] float rotateRatio = 5f;
    [SerializeField] float chaseSpeed = 4f;

    Rigidbody rb;
    bool isChasing;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 敵から見たプレイヤー位置ベクトル
        Vector3 sub = player.transform.position - rb.position;
        if (sub.magnitude <= senceRange && Vector3.Dot(sub, transform.forward) >= 0)
        {
            // プレイヤーのXZ方向ベクトルを計算
            Vector3 dirXZ = sub;
            dirXZ.y = 0f;
            dirXZ.Normalize();

            // 補間して新しい方向を設定
            Vector3 targetForward = Vector3.Slerp(transform.forward, dirXZ, rotateRatio * Time.deltaTime);
            rb.MoveRotation(Quaternion.LookRotation(targetForward));
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        if(isChasing)
        {
            rb.linearVelocity = transform.forward * chaseSpeed;
        }
    }
}

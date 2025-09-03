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

            // ������ׂ��������߂����
            if (c != this && len < avoidDistance)
            {
                // ����������痣�������։�]
                leaveRotate(c.transform.position - transform.position);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach(var contact in collision.contacts)
        {
            if (contact.otherCollider.gameObject.layer == 6) // ���C���[����
            {
                var dir = (transform.forward - contact.normal) * 0.5f;
                leaveRotate(dir); // �@���̋t�����ƑO���̒��ԕ������痣���
            }
        }
    }

    // �w�肵���������痣�������ɉ�]����
    void leaveRotate(Vector3 dir)
    {
        Vector2 toFlat = new Vector2(dir.x, dir.z).normalized;
        Vector2 forwardFlat = new Vector2(transform.forward.x, transform.forward.z).normalized;

        // �O���������i�R�T�C���l�j
        float dot = Vector2.Dot(forwardFlat, toFlat);
        // 60�x�ȓ����ǂ����icos60��= 0.5�j
        bool isWithin60 = dot > 0.5f;

        // �E�������i�E�Ȃ�cross.z > 0, ���Ȃ�cross.z < 0�j
        float crossZ = Vector3.Cross(forwardFlat, toFlat).z;
        bool isRight60 = isWithin60 && crossZ < 0f;
        bool isLeft60 = isWithin60 && crossZ > 0f;

        if (isRight60)
        {
            // �E60�x�ȓ��Ȃ獶��]
            // �E��]
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, -rotateSpeed * Time.deltaTime, 0));
        }
        else if (isLeft60)
        {
            // ��60�x�ȓ��Ȃ�E��]
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotateSpeed * Time.deltaTime, 0));
        }
    }
}

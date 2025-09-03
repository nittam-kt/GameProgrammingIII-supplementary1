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
        // �G���猩���v���C���[�ʒu�x�N�g��
        Vector3 sub = player.transform.position - rb.position;
        if (sub.magnitude <= senceRange && Vector3.Dot(sub, transform.forward) >= 0)
        {
            // �v���C���[��XZ�����x�N�g�����v�Z
            Vector3 dirXZ = sub;
            dirXZ.y = 0f;
            dirXZ.Normalize();

            // ��Ԃ��ĐV����������ݒ�
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

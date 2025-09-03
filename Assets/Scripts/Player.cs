using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float accel;
    [SerializeField] float jumpSpeed;
    [SerializeField] float rotateRatio;
    [SerializeField] Animator animator;

    public int lifeMax => 10;

    const float damageTimeMax = 2f;

    Rigidbody rb;
    PlayerInput playerInput;
    bool isGrounding;
    bool canJump;
    float damageTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.sleepThreshold = 0;
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        var move = playerInput.actions["Move"].ReadValue<Vector2>();

        // カメラの前方向と右方向
        var forward = Camera.main.transform.forward;
        var right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        // TODO:move3dをカメラの向きを考慮したベクトルにする
        var move3d = new Vector3(move.y, 0, -move.x);

        if (isGrounding && move3d != Vector3.zero)
        {
            rb.AddForce(move3d * accel, ForceMode.Acceleration);
            transform.forward = Vector3.Slerp(transform.forward, move3d, rotateRatio * Time.deltaTime);
        }

        if (canJump && playerInput.actions["Jump"].WasPressedThisFrame())
        {
            // TODO:rb.linearVelocityを変更してジャンプ
        }

        // アニメーターにスピードを与える
        animator.SetFloat("Speed", rb.linearVelocity.magnitude);

        if(damageTime > 0f)
        {
            damageTime -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        isGrounding = false;
        canJump = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (var contact in collision.contacts)
        {
            var normal = contact.normal;
            if(normal.y > 0.5f)
            {
                isGrounding = true;
            }
            if (normal.y > 0.9f)
            {
                canJump = true;
            }
        }
    }
}

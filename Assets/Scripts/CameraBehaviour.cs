using UnityEngine;
using UnityEngine.InputSystem;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] Transform lookTarget;
    [SerializeField] Vector3 offset;
    [SerializeField] float angleSpeed;

    PlayerInput playerInput;
    float yaw;
    float pitch;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion lookRotation = Quaternion.identity;
        if(Input.GetKey(KeyCode.I))
        {
            pitch += angleSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.K))
        {
            pitch -= angleSpeed * Time.deltaTime;
        }
        pitch = Mathf.Clamp(pitch, -30, 30.0f);
        if (Input.GetKey(KeyCode.J))
        {
            yaw += angleSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.L))
        {
            yaw -= angleSpeed * Time.deltaTime;
        }
        lookRotation.eulerAngles = new Vector3(pitch, yaw, 0);
        var ofs = lookRotation * offset;

        transform.position = lookTarget.position + ofs + new Vector3(0, offset.y, 0);
        transform.LookAt(lookTarget.position + new Vector3(0, offset.y,0));
    }
}

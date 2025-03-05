using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;

    private float horizontalInput;
    private float verticalInput;
    private float jumpInput;
    private Rigidbody playerRb;

    public float mouseSensitivity = 100f;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private float zRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) CameraManager.instance.SwitchCam();
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        jumpInput = Input.GetAxisRaw("Jump");
        Rotate();
    }

    private void FixedUpdate()
    {
        Vector3 movementZ = transform.forward * verticalInput;
        Vector3 movementX = transform.right * horizontalInput;
        Vector3 jumpMovement = transform.up * jumpInput;
        Vector3 movement = (movementX + movementZ + jumpMovement).normalized * acceleration /** Time.fixedDeltaTime*/;

        playerRb.AddForce(movement, ForceMode.Acceleration);
        if (playerRb.velocity.magnitude > maxSpeed) { playerRb.velocity = Vector3.ClampMagnitude(playerRb.velocity, maxSpeed); }
    }

    void Rotate() 
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        //float inputZ = Input.GetAxisRaw("Rotation") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation -= mouseX;
        //zRotation -= inputZ;

        transform.rotation = Quaternion.Euler(xRotation, -yRotation, zRotation);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class Player : MonoBehaviour
{
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;

    private float horizontalInput;
    private float verticalInput;
    private float jumpInput;
    private Rigidbody playerRb;

    public float mouseSensitivity = 100f;

    [SerializeField] Transform primaryWeaponSlot;
    [SerializeField] Transform secondaryWeaponSlot;
    private Gun gun;
    GameObject gunInstance;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        transform.rotation = Quaternion.identity;
        playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) CameraManager.instance.SwitchCam();
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        jumpInput = Input.GetAxisRaw("Jump");
        Rotate();

        gun?.Cooldown();

        if (Input.GetMouseButtonDown(0))
        {
            //gun?.Shoot(gunInstance.GetComponent<GunController>().firePoint);
            gun?.Shoot(Camera.main.transform);
        }
    }

    private void FixedUpdate()
    {
        Vector3 movementZ = transform.forward * verticalInput;
        Vector3 movementX = transform.right * horizontalInput;
        Vector3 jumpMovement = transform.up * jumpInput;
        Vector3 movement = (movementX + movementZ + jumpMovement).normalized * acceleration /** Time.fixedDeltaTime**/;

        //playerRb.AddForce(movement, ForceMode.Acceleration);
        //if (playerRb.velocity.magnitude > maxSpeed) { playerRb.velocity = Vector3.ClampMagnitude(playerRb.velocity, maxSpeed); }
        playerRb.velocity = movement;
    }


    [SerializeField] private float snapSpeed = 0.2f;
    void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        float inputZ = Input.GetAxisRaw("Rotation") * mouseSensitivity * Time.deltaTime;

        // Crea quaternioni incrementali per ogni asse
        Quaternion rotX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotY = Quaternion.AngleAxis(mouseX, Vector3.up);
        Quaternion rotZ = Quaternion.AngleAxis(-inputZ, Vector3.forward);

        float rot = transform.eulerAngles.z;
        float snapAngle = Mathf.Round(rot / 90) * 90 % 360;

        if (inputZ == 0 && rot != snapAngle)
        {
            float diff = Mathf.DeltaAngle(rot, snapAngle); // Trova la differenza angolare più breve

            if (Mathf.Abs(diff) < 1 && Mathf.Abs(diff) != 0)
            {
                // Se la differenza è minore di 2 gradi, imposta direttamente l'angolo
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, snapAngle);
            }
            else
            {
                // Altrimenti, ruota gradualmente nella direzione più corta
                rotZ = Quaternion.AngleAxis(Mathf.Sign(diff) * snapSpeed, Vector3.forward);
            }
        }

        // Aggiorna la rotazione evitando la conversione in angoli di Eulero
        transform.localRotation = transform.localRotation * rotY * rotX * rotZ;
    }

    public void EquipWeapon(WeaponData weapon)
    {
        if (weapon.type.Equals(GunType.Primary))
            gunInstance = Instantiate(weapon.Gun, primaryWeaponSlot.position, primaryWeaponSlot.rotation, primaryWeaponSlot);
        if (weapon.type.Equals(GunType.Secondary))
            gunInstance = Instantiate(weapon.Gun, secondaryWeaponSlot.position, secondaryWeaponSlot.rotation, secondaryWeaponSlot);
        gun = new Gun(weapon, gunInstance);
    }
}

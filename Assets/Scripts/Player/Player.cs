using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour, IDamageable
{
    public static Player Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }

    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;

    private float horizontalInput;
    private float verticalInput;
    private float jumpInput;
    private Rigidbody playerRb;

    public float mouseSensitivity = 100f;

    private Gun primaryGun;
    private Gun secondaryGun;

    [SerializeField] private List<Transform> firepoints1 = new List<Transform>();
    [SerializeField] private List<Transform> firepoints2 = new List<Transform>();
    [SerializeField] private List<Transform> firepoints3 = new List<Transform>();

    public int energy;
    public int shield;

    public int currentHp { get; set; }
    public int startingHP;
    private Vector3 spawnPoint;
    public void TakeDamage(int damage)
    {
        shield -= damage;
        FindAnyObjectByType<HudUI>().SetShield(shield);
        if (shield <= 0)
        {
            shield = 100;
            currentHp--;

            // UI
            FindAnyObjectByType<HudUI>().SetHealth(currentHp);
            FindAnyObjectByType<HudUI>().SetShield(shield);

            if (currentHp < 0)
            {
                //ResetPlayer();
                //FindAnyObjectByType<HudUI>().SetHealth(currentHp);
                // UI YOU LOSE
                print("Hai perso");
            }
        }
    }

    private void ResetPlayer()
    {
        shield = 100;
        energy = 100;
        transform.position = spawnPoint;
        currentHp = startingHP;
    }

    private void Start()
    {
        currentHp = startingHP;
        spawnPoint = transform.position;
        transform.rotation = Quaternion.identity;
        playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (UIManager.instance.GetCurrentActiveUI() != UIManager.GameUI.HUD) return;

        if (playerInside) ChargingEnergy();
        if (Input.GetKeyDown(KeyCode.R)) CameraManager.instance.SwitchCam();
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        jumpInput = Input.GetAxisRaw("Jump");
        Rotate();

        primaryGun?.Cooldown();
        secondaryGun?.Cooldown();

        if (Input.GetMouseButtonDown(0))
        {
            //gun?.Shoot(gunInstance.GetComponent<GunController>().firePoint);
            primaryGun?.Shoot();
        }
        if (Input.GetMouseButtonDown(1))
        {
            //gun?.Shoot(gunInstance.GetComponent<GunController>().firePoint);
            secondaryGun?.Shoot();
        }
    }

    private void FixedUpdate()
    {
        Vector3 movementZ = transform.forward * verticalInput;
        Vector3 movementX = transform.right * horizontalInput;
        Vector3 jumpMovement = transform.up * jumpInput;
        Vector3 movement = (movementX + movementZ + jumpMovement).normalized * acceleration /** Time.fixedDeltaTime**/;

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
        if (weapon.weaponType.Equals(GunType.Primary))
        {
            //gunInstance = Instantiate(weapon.Gun, primaryWeaponSlot.position, primaryWeaponSlot.rotation, primaryWeaponSlot);
            switch (weapon.firePointType)
            {
                case FirePointType.One:
                    primaryGun = new Gun(weapon, ShooterType.Player, firepoints1);
                    break;
                case FirePointType.Two:
                    primaryGun = new Gun(weapon, ShooterType.Player, firepoints2);
                    break;
                case FirePointType.Four:
                    primaryGun = new Gun(weapon, ShooterType.Player, firepoints3);
                    break;
            }
        }
        if (weapon.weaponType.Equals(GunType.Secondary))
        {
            //gunInstance = Instantiate(weapon.Gun, secondaryWeaponSlot.position, secondaryWeaponSlot.rotation, secondaryWeaponSlot);
            secondaryGun = new Gun(weapon, ShooterType.Player, firepoints1);
        }
    }

    public void CollectKey(ItemData item)
    {
        print("item collected: " + item.name);
        GameManager.instance.AddKeys();
        print("keys: " + GameManager.instance.keysCollected);
    }

    public void CollectHostage(HostageData item)
    {
        print("item collected: " + item.name);
        GameManager.instance.AddHostage();
    }

    public void CollectAmmo(AmmoData ammo)
    {
        // TODO DA RIFARE
        //if (ammo.gunType.Equals(GunType.Primary))
        //    primaryGun?.AddPrimaryAmmo(ammo.ammo);
        //if (ammo.gunType.Equals(GunType.Secondary))
        //    secondaryGun?.AddSecondaryAmmo(ammo.ammo);
    }

    public void CollectPU_Shield(PowerUpData powerUp)
    {
        shield += powerUp.value;
        if (shield > 100)
        {
            shield = 100;
        }
        Debug.Log("Added: " + powerUp.value);
        Debug.Log("- shield: " + shield);
    }

    public void CollectPU_Energy(PowerUpData powerUp)
    {
        energy += powerUp.value;
        if (energy > 100)
        {
            energy = 100;
        }
        Debug.Log("Added: " + powerUp.value);
        Debug.Log("- energy: " + energy);
    }

    // Incremento desiderato per secondo (modificabile dall'Inspector)
    public float incrementPerSecond = 1f;

    // Variabile interna per accumulare incrementi parziali
    private float accumulator = 0f;
    // Flag per verificare se il player è all'interno della zona
    private bool playerInside = false;
    public void ChargingEnergy()
    {
        accumulator += incrementPerSecond * Time.deltaTime;
        // Quando l'accumulatore supera 1, si incrementa il valore intero
        if (accumulator >= 1f)
        {
            energy += 1;
            accumulator = 0;
        }
    }

    public int GetCurrentEnergy() { return energy; }

    public void UseEnergy(int energyUsed) { energy -= energyUsed; FindAnyObjectByType<HudUI>().SetEnergy(energy); }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnergyCharge"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnergyCharge"))
        {
            playerInside = false;
            accumulator = 0;
        }
    }
}

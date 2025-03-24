using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public class Gun
{
    float fireCooldown;
    public bool canShoot { get; private set; }

    // Ammo
    public int currentAmmo;
    public WeaponData gun { get; private set; }
    public GameObject projectile;

    private List<Transform> firePoints;

    public Gun(WeaponData gun, ShooterType shooter, List<Transform> firePoints)
    {
        this.gun = gun;
        projectile = gun.projectilePref;
        this.firePoints = firePoints;

        fireCooldown = 1f / gun.fireRate;
        currentAmmo = gun.startingAmmo;
    }

    public void Cooldown()
    {
        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f)
        {
            canShoot = true;
        }
    }

    public void Shoot(Transform firePoint)
    {
        if (canShoot)
        {
            Debug.DrawRay(firePoint.position, Camera.main.transform.forward * 100, Color.red, 2f);
            // Using Ammo
            if (gun.startingAmmo != 0)
            {
                if (currentAmmo > 0)
                {
                    // Sparo
                    ShootProjectiles();

                    Debug.Log("Sparo con proiettili");
                    currentAmmo--;
                }
            }
            else if (gun.weaponType != GunType.Secondary) // Using Energy
            {
                if (Player.Instance.GetCurrentEnergy() - gun.energyUsed >= 0)
                {
                    // Sparo
                    ShootProjectiles();

                    Debug.Log("Sparo con energia");
                    Player.Instance.UseEnergy(gun.energyUsed);
                }
            }
            // cooldown
            fireCooldown = 1f / gun.fireRate;
            canShoot = false;
        }
    }

    private void ShootProjectiles()
    {
        foreach (var firepoint in firePoints)
        {

        }
    }

    public void AddAmmo(int value)
    {
        currentAmmo += value;
    }
}

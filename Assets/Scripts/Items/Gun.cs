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
    private ShooterType shooter;
    private List<Transform> firePoints;

    private Vector3 projectileDirection;

    public Gun(WeaponData gun, ShooterType shooter, List<Transform> firePoints)
    {
        this.gun = gun;
        projectile = gun.projectilePref;
        this.firePoints = firePoints;
        this.shooter = shooter;

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

    public void Shoot()
    {
        if (canShoot)
        {
            // Using Ammo
            if (gun.startingAmmo != 0 || shooter == ShooterType.Enemy)
            {
                if (currentAmmo > 0)
                {
                    // Sparo
                    ShootProjectiles();

                    Debug.Log("Sparo con proiettili");
                    if (shooter != ShooterType.Enemy)
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
            GameObject obj = Object.Instantiate(projectile, firepoint.position, Quaternion.identity);
            obj.GetComponent<Projectile>().SetDamage(gun.damage);
        }
    }

    public void AddAmmo(int value)
    {
        currentAmmo += value;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public class Gun
{
    float fireCooldown;
    public bool canShoot { get; private set; }

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
                if (gun.weaponType == GunType.Primary && (Player.Instance.primaryAmmo > 0) || shooter == ShooterType.Enemy)
                {
                    // Sparo
                    ShootProjectiles();

                    if (shooter != ShooterType.Enemy)
                        Player.Instance.primaryAmmo--;
                }
                else if (gun.weaponType == GunType.Secondary && (Player.Instance.secondaryAmmo > 0) || shooter == ShooterType.Enemy)
                {
                    // Sparo
                    ShootProjectiles();

                    if (shooter != ShooterType.Enemy)
                        Player.Instance.secondaryAmmo--;
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
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public class Gun
{
    float fireCooldown;
    public bool canShoot { get; private set; }

    // Ammo
    public int currentPrimaryAmmo;
    public int currentSecondaryAmmo;

    public Gun(WeaponData gun, GameObject gunInstance)
    {
        this.gun = gun;

        fireCooldown = 1f / gun.fireRate;
    }

    public WeaponData gun { get; private set; }

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
            Debug.Log("shooting");

            // spara dal centro
            Ray ray = new Ray(firePoint.position, Camera.main.transform.forward * 100);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag(Tags.Enemy))
                {
                    // hitta un collider e fa danno
                    Debug.Log("hittato nemico");
                }
            }

            Debug.DrawRay(firePoint.position, Camera.main.transform.forward * 100, Color.red, 2f);

            // primary ammo???
            currentPrimaryAmmo--;

            // cooldown
            fireCooldown = 1f / gun.fireRate;
            canShoot = false;
        }
    }

    public void AddPrimaryAmmo(int value)
    {
        currentPrimaryAmmo += value;
    }

    public void AddSecondaryAmmo(int value)
    {
        currentSecondaryAmmo += value;
    }
}

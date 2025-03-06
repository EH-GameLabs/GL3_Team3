using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public class Gun
{
    float fireCooldown;
    public bool canShoot { get; private set; }

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
            Debug.Log(Tags.Enemy);
            // spara dal centro
            Ray ray = new Ray(firePoint.position, Camera.main.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag(Tags.Enemy))
                {
                    // hitta un collider e fa danno
                    Debug.Log("hittato nemico");
                }
            }

            Debug.DrawRay(firePoint.position, Camera.main.transform.forward * 100, Color.red, 2f);

            // cooldown
            fireCooldown = 1f / gun.fireRate;
            canShoot = false;
        }
        Debug.Log("Nope");
    }
}

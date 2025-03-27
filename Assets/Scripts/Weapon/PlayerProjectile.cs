using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    private Vector3 destination;
    private Vector3 direction;

    private float destructionTimer = 15f;

    private void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore))
        {
            SetDestination(hit.point);
        }

        direction = (destination - transform.position).normalized;
    }

    private void Update()
    {
        if (UIManager.instance.GetCurrentActiveUI() != UIManager.GameUI.HUD) return;
        transform.Translate(speed * Time.deltaTime * direction);

        UpdateTimer();
    }

    private void UpdateTimer()
    {
        destructionTimer -= Time.deltaTime;
        if (destructionTimer < 0)
        {
            Destroy(gameObject);
        }
    }

    public override void SetDestination(Vector3 destination)
    {
        this.destination = destination;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() != null) return;

        if (collision.gameObject.GetComponent<IDamageable>() != null)
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}

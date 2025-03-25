using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    private Vector3 destination;
    private Vector3 direction;

    private void Start()
    {
        SetDestination(Player.Instance.transform.position);
        direction = (destination - transform.position).normalized;
    }

    private void Update()
    {
        if (UIManager.instance.GetCurrentActiveUI() != UIManager.GameUI.HUD) return;
        transform.Translate(speed * Time.deltaTime * direction);
    }

    public override void SetDestination(Vector3 destination)
    {
        this.destination = destination;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}

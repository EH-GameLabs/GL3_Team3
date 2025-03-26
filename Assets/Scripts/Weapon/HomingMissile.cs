using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : Projectile
{
    private Vector3 destination;
    private Vector3 direction;
    Enemy[] enemies;
    GameObject closest;

    private void Start()
    {
        enemies = FindObjectsOfType<Enemy>();
        if (enemies.Length == 0) { Destroy(gameObject); return; }
        closest = GetClosest();
    }

    private void Update()
    {
        if (UIManager.instance.GetCurrentActiveUI() != UIManager.GameUI.HUD) return;
        SetDestination(closest.transform.position);
        direction = (destination - transform.position).normalized;
        transform.Translate(speed * Time.deltaTime * direction);
    }

    public override void SetDestination(Vector3 destination)
    {
        this.destination = destination;
    }

    private GameObject GetClosest()
    {
        GameObject tmp = enemies[0].gameObject;

        foreach (var g in enemies)
        {
            if (Vector3.Distance(Player.Instance.transform.position, g.transform.position) <
                Vector3.Distance(Player.Instance.transform.position, tmp.transform.position))
            {
                tmp = g.gameObject;
            }
        }

        return tmp;
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
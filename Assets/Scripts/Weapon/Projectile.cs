using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected float speed;
    protected int damage;
    public void SetDamage(int damage) { this.damage = damage; }
    public abstract void SetDestination(Vector3 destination);
}

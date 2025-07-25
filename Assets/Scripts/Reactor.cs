using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : MonoBehaviour, IDamageable
{
    [SerializeField] int hp;
    public int currentHp { get; set; }

    private void Start()
    {
        currentHp = hp;
    }

    public void TakeDamage(int damage)
    {
        FindAnyObjectByType<HudUI>().ShowHitmarker();
        currentHp -= damage;
        if (currentHp <= 0) { Destroy(gameObject); }
    }
}

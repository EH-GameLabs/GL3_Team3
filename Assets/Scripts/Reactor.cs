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
        currentHp -= damage;
        if (currentHp < 1) { print("Win!"); }
    }
}

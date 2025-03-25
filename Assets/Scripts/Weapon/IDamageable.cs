using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    int currentHp { get; set; }
    public void TakeDamage(int damage);
}

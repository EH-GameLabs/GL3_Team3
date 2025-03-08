using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObject/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int hp;
    public int attackDamage;
    public float fireRate;
    public int pointsOnDeath;
    public Drop drop;
}

[System.Serializable]
public class Drop
{
    public GameObject dropObj;
    [Range(0, 1)] public float weight;
}
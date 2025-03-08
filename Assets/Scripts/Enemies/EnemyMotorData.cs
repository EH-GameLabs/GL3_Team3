using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyMotorData", menuName = "ScriptableObject/EnemyMotorData")]
public class EnemyMotorData : ScriptableObject
{
    public float movementSpeed;
    public EnemyBehaviour enemyIdle;
    public EnemyBehaviour enemyChasing;
}

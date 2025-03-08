using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EnemyMotorData enemyMotor;

    private GameObject player;
    private EnemyState enemyState;

    private int hp;
    private bool isPlayerInRange;
    private bool canShoot;

    private void Start()
    {
        hp = enemyData.hp;

        enemyState = EnemyState.Idle;
        player = FindAnyObjectByType<Player>().gameObject;
    }

    private void Update()
    {
        // se il game è !active return

        // SHOOTING
        if (isPlayerInRange && canShoot)
        {
            Shoot();
            canShoot = false;
        }

        // AI STATES
        CheckStates();

        // MOVEMENT
        if (enemyMotor != null)
        {
            if (enemyMotor.movementSpeed > 0)
                Move();
        }
    }

    public void CheckStates()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Chase:
                break;
            case EnemyState.Attack:
                break;
        }
    }

    #region MOVEMENT
    public void Move()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                PatrolArea();
                break;
            case EnemyState.Chase:
                ChasePlayer();
                break;
            case EnemyState.Attack:
                AttackPlayer();
                break;
        }
    }

    private void PatrolArea()
    {
        print("patrolling");
    }

    private void ChasePlayer()
    {
        print("chasing");
        // move towards player

        if (Vector3.Distance(transform.position, player.transform.position) < 3f)
        {
            print("In AttackRange");
            SetState(EnemyState.Attack);
        }
    }

    private void AttackPlayer()
    {
        print("attacking");
        // stop movement and attack player

        if (Vector3.Distance(transform.position, player.transform.position) >= 3f)
        {
            print("Out AttackRange");
            SetState(EnemyState.Chase);
        }
    }
    #endregion

    #region SHOOTING
    public void Shoot()
    {
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        yield return new WaitForSeconds(enemyData.fireRate);
        canShoot = true;
    }
    #endregion

    #region SETTERS & GETTERS
    public void DoDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            print("Enemy defeated");
            Destroy(gameObject);
        }
    }

    public void SetState(EnemyState enemyState) { this.enemyState = enemyState; }
    #endregion

    #region COLLISIONS
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            SetState(EnemyState.Chase);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            SetState(EnemyState.Idle);
        }
    }

    #endregion
}

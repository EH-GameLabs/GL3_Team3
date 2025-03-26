using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Scriptable Objects")]
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EnemyMotorData enemyMotor;
    [SerializeField] private WeaponData weaponData;

    [Header("Sight Stats")]
    [SerializeField][Range(0f, 60f)] private int angle;
    [SerializeField][Range(0f, 60f)] private int nrRay;
    [SerializeField] private float attackRange;
    [SerializeField] List<Transform> firepoint;

    private GameObject player;
    private EnemyState enemyState;

    public int currentHp { get; set; }
    private bool isPlayerInRange;

    // Player Movement
    private Vector3 destination;
    private bool isRotating;

    private Gun enemyGun;

    private void Start()
    {
        currentHp = enemyData.hp;
        destination = transform.position;

        enemyState = EnemyState.Idle;
        player = FindAnyObjectByType<Player>().gameObject;
        enemyGun = new Gun(weaponData, ShooterType.Enemy, firepoint);
    }

    private void Update()
    {
        // se il game è !active return
        if (UIManager.instance.GetCurrentActiveUI() != UIManager.GameUI.HUD) return;

        enemyGun.Cooldown();

        // AI STATES
        CheckStates();

        // MOVEMENT
        if (enemyMotor != null)
        {
            if (enemyMotor.movementSpeed > 0)
                Move();
        }

        //CheckIfAlive();
    }

    private void CheckIfAlive()
    {
        if (currentHp <= 0)
        {
            GameManager.instance.AddScore(enemyData.pointsOnDeath);
            Destroy(gameObject);
        }
    }

    public void CheckStates()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                if (isPlayerInRange)
                    CanSeePlayer();
                break;
            case EnemyState.Chase:
                transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
                CanSeePlayer();
                break;
            case EnemyState.Attack:
                transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
                AttackPlayer();
                break;
        }
    }

    private void CanSeePlayer()
    {
        if (player == null) return;

        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        Ray ray;

        for (int i = 0; i < nrRay; i++)
        {
            float angleOffset = (i - (nrRay / 2f)) * ((float)angle / (float)nrRay);
            Quaternion rotation = Quaternion.Euler(0, angleOffset, 0);
            Vector3 rayDirection = rotation * directionToPlayer;

            Debug.DrawRay(transform.position, rayDirection * 20, Color.red);

            ray = new Ray(transform.position, rayDirection);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag(Tags.Player))
                {
                    if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
                    {
                        print("In AttackRange");
                        SetState(EnemyState.Attack);
                        return;
                    }
                }
                if (hit.collider.CompareTag(Tags.Player))
                {
                    SetState(EnemyState.Chase);
                    return;
                }
            }
        }
        SetState(EnemyState.Idle);

    }

    #region AI STATES
    private void PatrolArea()
    {
        if (IsArrived(destination))
        {
            destination = CalculateNextPatrolPoint();

            // Calcola la direzione di movimento dal punto attuale verso la destinazione
            Vector3 moveDir = (destination - transform.position).normalized;

            //int layersToIgnore = LayerMask.GetMask("Seen");
            //int layerMask = ~layersToIgnore; // Ignora questi layer
            //print("Layer: " + layerMask);

            if (Physics.Raycast(new Ray(transform.position, moveDir), out RaycastHit hit, Vector3.Distance(transform.position, destination), ~ignoreMe))
            {
                // Per ciascun asse, sottraggo 1 o -1 in base alla direzione
                destination = hit.collider.transform.position - new Vector3(
                    Mathf.Sign(moveDir.x),
                    Mathf.Sign(moveDir.y),
                    Mathf.Sign(moveDir.z)
                );
            }
            isRotating = true;
            StartCoroutine(RotateToDirection(destination, 0.5f));
            print("starting rotation");
        }
        else
        {
            Debug.DrawLine(transform.position, destination, Color.red);
            if (!isRotating)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime);
                //transform.rotation = Quaternion.LookRotation(destination - transform.position);
            }
        }
    }

    public IEnumerator RotateToDirection(Vector3 targetDirection, float duration)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection - transform.position);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        isRotating = false;
    }

    private bool IsArrived(Vector3 destination)
    {
        if (Vector3.Distance(transform.position, destination) < 0.5f)
        {
            return true;
        }
        return false;
    }

    private Vector3 CalculateNextPatrolPoint()
    {
        // Calcola le distanze disponibili in ciascuna direzione
        float dForward = GetDistanceInDirection(transform.position, transform.forward);
        float dBackward = GetDistanceInDirection(transform.position, -transform.forward);
        float dRight = GetDistanceInDirection(transform.position, transform.right);
        float dLeft = GetDistanceInDirection(transform.position, -transform.right);
        float dUp = GetDistanceInDirection(transform.position, transform.up);
        float dDown = GetDistanceInDirection(transform.position, -transform.up);

        // Calcola l'offset per la direzione avanti-indietro
        Vector3 offsetZ = Vector3.zero;
        if (dForward + dBackward > 0)
        {
            float pZ = dForward / (dForward + dBackward); // probabilità di andare avanti
            if (Random.value < pZ)
            {
                float distance = Random.Range(0f, dForward - 2f);
                offsetZ = transform.forward * distance;
            }
            else
            {
                float distance = Random.Range(0f, dBackward - 2f);
                offsetZ = -transform.forward * distance;
            }
        }
        else
        {
            Debug.LogWarning("HUH? dForward + dBackward <= 0");
        }

        // Calcola l'offset per la direzione destra-sinistra
        Vector3 offsetX = Vector3.zero;
        if (dRight + dLeft > 0)
        {
            float pX = dRight / (dRight + dLeft); // probabilità di andare a destra
            if (Random.value < pX)
            {
                float distance = Random.Range(0f, dRight - 2f);
                offsetX = transform.right * distance;
            }
            else
            {
                float distance = Random.Range(0f, dLeft - 2f);
                offsetX = -transform.right * distance;
            }
        }
        else
        {
            Debug.LogWarning("HUH? dRight + dLeft <= 0");
        }

        // Calcola l'offset per la direzione su-giu
        Vector3 offsetY = Vector3.zero;
        if (dUp + dDown > 0)
        {
            float pY = dUp / (dUp + dDown); // probabilità di andare in alto
            if (Random.value < pY)
            {
                float distance = Random.Range(0f, dUp - 2f);
                offsetY = transform.up * distance;
            }
            else
            {
                float distance = Random.Range(0f, dDown - 2f);
                offsetY = -transform.up * distance;
            }
        }
        else
        {
            Debug.LogWarning("HUH? dUp + dDown <= 0");
        }

        // Somma gli offset per ottenere il punto di destinazione
        Vector3 nextPoint = transform.position + offsetX + offsetY + offsetZ;
        return nextPoint;
    }

    public LayerMask ignoreMe;

    private float GetDistanceInDirection(Vector3 origin, Vector3 direction)
    {
        Ray ray = new Ray(origin, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~ignoreMe))
        {
            return hit.distance;
        }
        return 0f;
    }

    private void ChasePlayer()
    {
        print("chasing");
        Debug.DrawLine(transform.position, destination, Color.red);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime);
    }

    private void AttackPlayer()
    {
        enemyGun.Shoot();

        if (Vector3.Distance(transform.position, player.transform.position) >= attackRange)
        {
            print("Out AttackRange");
            SetState(EnemyState.Chase);
        }
    }
    #endregion

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
        }
    }

    #region SETTERS & GETTERS
    public void DoDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
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
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            isPlayerInRange = false;
            SetState(EnemyState.Idle);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp < 1) 
        {
            GameManager.instance.AddScore(enemyData.pointsOnDeath);
            Destroy(gameObject);
        }
    }

    #endregion
}

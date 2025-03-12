using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EnemyMotorData enemyMotor;

    [Header("Sight Stats")]
    [SerializeField][Range(0f, 60f)] private int angle;
    [SerializeField][Range(0f, 60f)] private int nrRay;

    private GameObject player;
    private EnemyState enemyState;

    private int hp;
    private bool isPlayerInRange;
    private bool isPlayerInAttackRange;
    private bool canShoot;

    // Player Movement
    private Ray rayRight;
    private Ray rayLeft;
    private Ray rayUp;
    private Ray rayDown;
    private Ray rayForward;
    private Ray rayBackward;
    private Vector3 destination;

    private void Start()
    {
        hp = enemyData.hp;
        destination = transform.position;

        enemyState = EnemyState.Idle;
        player = FindAnyObjectByType<Player>().gameObject;
    }

    private void Update()
    {
        // se il game è !active return

        // SHOOTING
        if (isPlayerInAttackRange && canShoot)
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
                if (isPlayerInRange)
                    CanSeePlayer();
                break;
            case EnemyState.Chase:
                CanSeePlayer();
                break;
            case EnemyState.Attack:
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
        print("destination: " + destination);

        if (IsArrived(destination))
        {
            destination = CalculateNextPatrolPoint();

            // Calcola la direzione di movimento dal punto attuale verso la destinazione
            Vector3 moveDir = (destination - transform.position).normalized;

            if (Physics.Raycast(new Ray(transform.position, moveDir), out RaycastHit hit, Vector3.Distance(transform.position, destination)))
            {
                print("hit: " + hit.collider.transform.position);

                // Per ciascun asse, sottraggo 1 o -1 in base alla direzione
                destination = hit.collider.transform.position - new Vector3(
                    Mathf.Sign(moveDir.x),
                    Mathf.Sign(moveDir.y),
                    Mathf.Sign(moveDir.z)
                );
            }
        }
        else
        {
            Debug.DrawLine(transform.position, destination, Color.red);
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime);
        }
    }


    private bool IsArrived(Vector3 destination)
    {
        if (Vector3.Distance(transform.position, destination) < 1f)
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
    private float GetDistanceInDirection(Vector3 origin, Vector3 direction)
    {
        Ray ray = new Ray(origin, direction);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.distance;
        }
        return 0f;
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
        //print("attacking");
        // stop movement and attack player

        if (Vector3.Distance(transform.position, player.transform.position) >= 3f)
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

    #endregion
}

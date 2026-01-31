using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    private NavMeshAgent agent;
    private Transform player;

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolWaitTime = 2f;
    [SerializeField] private float patrolSpeed = 2f;
    private int currentPatrolIndex;
    private bool isWaiting;

    [Header("Chase Settings")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float chaseSpeed = 5f;
    private bool isChasing;

    [Header("Attack Settings")]
    [SerializeField] private int damageToPlayer = 10;
    [SerializeField] private float attackCooldown = 1f;
    private float lastAttackTime;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("Player tidak ditemukan!");
            enabled = false;
            return;
        }
        player = playerObj.transform;

        agent.speed = patrolSpeed;

        if (patrolPoints != null && patrolPoints.Length > 0)
            SetNextPatrolDestination();
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // ===== CHASE =====
        if (distance <= detectionRange)
        {
            if (!isChasing)
            {
                isChasing = true;
                agent.speed = chaseSpeed;
            }
            agent.SetDestination(player.position);
        }
        // ===== PATROL =====
        else
        {
            if (isChasing)
            {
                isChasing = false;
                agent.speed = patrolSpeed;
                SetNextPatrolDestination();
            }

            if (!isWaiting && patrolPoints.Length > 0)
            {
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    StartCoroutine(WaitAtPatrolPoint());
                }
            }
        }
    }

    private void SetNextPatrolDestination()
    {
        if (patrolPoints.Length == 0) return;

        int nextIndex;
        do
        {
            nextIndex = Random.Range(0, patrolPoints.Length);
        }
        while (patrolPoints.Length > 1 && nextIndex == currentPatrolIndex);

        currentPatrolIndex = nextIndex;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    private IEnumerator WaitAtPatrolPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(patrolWaitTime);
        isWaiting = false;
        SetNextPatrolDestination();
    }

    // ===== DAMAGE KE PLAYER =====
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                lastAttackTime = Time.time;

                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageToPlayer);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolWaitTime = 2f;
    [SerializeField] private float patrolSpeed = 2f;
    private int currentPatrolIndex;
    private bool isWaiting;

    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float chaseSpeed = 5f;
    private bool isChasing;

    private bool hasHitPlayer = false;

    private Vector3 startPosition; // 🔥 posisi awal enemy

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        startPosition = transform.position; // simpan posisi awal

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

        // ===== CHASE PLAYER =====
        if (distance <= detectionRange)
        {
            isChasing = true;
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
            return;
        }

        // ===== KELUAR JARAK → BALIK KE POS AWAL =====
        if (isChasing)
        {
            isChasing = false;
            agent.speed = patrolSpeed;
            agent.SetDestination(startPosition);
            return;
        }

        // ===== PATROL SETELAH SAMPAI =====
        if (!isWaiting && patrolPoints.Length > 0)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                StartCoroutine(WaitAtPatrolPoint());
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

    // ===== SENTUH PLAYER → DAMAGE + ENEMY HANCUR =====
    private void OnTriggerEnter(Collider other)
    {
        if (hasHitPlayer) return;
        if (!other.CompareTag("Player")) return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage();
        }

        hasHitPlayer = true;
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

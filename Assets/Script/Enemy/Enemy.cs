using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    [Header("Patrol")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolWaitTime = 2f;
    [SerializeField] private float patrolSpeed = 2f;
    private int currentPatrolIndex;
    private bool isWaiting;

    [Header("Chase")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float chaseSpeed = 5f;

    [Header("Audio")]
    [SerializeField] private AudioClip chaseSFX;

    private bool isChasing;
    private bool hasHitPlayer;
    private bool isChaseSFXPlaying;

    private Vector3 startPosition;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;

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

        // ================= CHASE =================
        if (distance <= detectionRange)
        {
            if (!isChasing)
                StartChase();

            agent.SetDestination(player.position);
            return;
        }

        // ================= STOP CHASE =================
        if (isChasing)
        {
            StopChase();
            agent.SetDestination(startPosition);
            return;
        }

        // ================= PATROL =================
        if (!isWaiting && patrolPoints.Length > 0)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                StartCoroutine(WaitAtPatrolPoint());
            }
        }
    }

    // ================= CHASE CONTROL =================
    private void StartChase()
    {
        isChasing = true;
        agent.speed = chaseSpeed;
        PlayChaseSFX();
    }

    private void StopChase()
    {
        isChasing = false;
        agent.speed = patrolSpeed;
        StopChaseSFX();
    }

    // ================= AUDIO =================
    private void PlayChaseSFX()
    {
        if (isChaseSFXPlaying) return;
        if (AudioManager.Instance == null || chaseSFX == null) return;

        AudioSource sfx = AudioManager.Instance.sfxSource;
        sfx.clip = chaseSFX;
        sfx.loop = true;
        sfx.Play();

        isChaseSFXPlaying = true;
    }

    private void StopChaseSFX()
    {
        if (!isChaseSFXPlaying) return;
        if (AudioManager.Instance == null) return;

        AudioManager.Instance.sfxSource.Stop();
        isChaseSFXPlaying = false;
    }

    // ================= PATROL =================
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

    // ================= HIT PLAYER =================
    private void OnTriggerEnter(Collider other)
    {
        if (hasHitPlayer) return;
        if (!other.CompareTag("Player")) return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
            playerHealth.TakeDamage();

        hasHitPlayer = true;
        StopChaseSFX();
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

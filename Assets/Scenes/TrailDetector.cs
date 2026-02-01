using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TrailDetector : MonoBehaviour
{
    [Header("Settings")]
    public bool useDuration = true;
    public float activeDuration = 5.0f;
    public float safeDistance = 5.0f;

    [Header("References")]
    public Transform player;
    public Transform[] targets;
    public RectTransform arrowUI;
    public Animator clownAnimator;

    private bool isActive = false;
    private float timer = 0f;

    void Start()
    {
        if (arrowUI != null) arrowUI.gameObject.SetActive(false);
    }

    void Update()
    {
        HandleInput();

        if (isActive)
        {
            Transform closestTarget = GetClosestTarget();

            if (closestTarget == null)
            {
                TurnOff();
                return;
            }

            float currentDistance = Vector3.Distance(player.position, closestTarget.position);

            if (currentDistance <= safeDistance)
            {
                TurnOff();
                Debug.Log($"Sudah sampai di lokasi aman: {closestTarget.name}");
                return;
            }

            UpdateArrowRotation(closestTarget);
            HandleDuration();
        }
    }

    void HandleInput()
    {
        if (Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame)
        {
            if (isActive)
            {
                TurnOff();
            }
            else
            {
                // INTEGRASI: Cek Manager sebelum menyalakan
                if (GlobalAbilityManager.Instance.TryActivateAbility())
                {
                    Transform closestTarget = GetClosestTarget();
                    if (closestTarget != null)
                    {
                        float currentDistance = Vector3.Distance(player.position, closestTarget.position);
                        if (currentDistance > safeDistance)
                        {
                            TurnOn();
                            if (clownAnimator != null)
                            {
                                clownAnimator.SetTrigger("Clown");
                            }
                        }
                        else
                        {
                            Debug.Log("Terlalu dekat.");
                            GlobalAbilityManager.Instance.FinishAbility(); // Cancel lock segera
                        }
                    }
                    else
                    {
                        Debug.Log("Tidak ada target.");
                        GlobalAbilityManager.Instance.FinishAbility(); // Cancel lock segera
                    }
                }
            }
        }
    }

    Transform GetClosestTarget()
    {
        if (targets == null || targets.Length == 0) return null;
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPos = player.position;

        foreach (Transform potentialTarget in targets)
        {
            if (potentialTarget == null) continue;
            Vector3 directionToTarget = potentialTarget.position - currentPos;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }

    void TurnOn()
    {
        isActive = true;
        timer = activeDuration;
        if (arrowUI != null) arrowUI.gameObject.SetActive(true);
    }

    void TurnOff()
    {
        if (!isActive) return;

        isActive = false;
        if (arrowUI != null) arrowUI.gameObject.SetActive(false);

        // INTEGRASI: Lepas kunci
        GlobalAbilityManager.Instance.FinishAbility();
    }

    void UpdateArrowRotation(Transform currentTarget)
    {
        if (player == null || currentTarget == null || arrowUI == null) return;
        Vector3 directionToTarget = currentTarget.position - player.position;
        directionToTarget.y = 0;
        float angle = Vector3.SignedAngle(player.forward, directionToTarget, Vector3.up);
        arrowUI.localRotation = Quaternion.Euler(0, 0, -angle);
    }

    void HandleDuration()
    {
        if (useDuration)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                TurnOff();
            }
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TrailDetector : MonoBehaviour
{
    [Header("Settings")]
    public bool useDuration = true;
    public float activeDuration = 5.0f;
    public float safeDistance = 5.0f; // Jarak aman (meter). Jika lebih dekat dari ini, sistem mati.

    [Header("References")]
    public Transform player;
    public Transform target;
    public RectTransform arrowUI;

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
            // Cek jarak secara real-time saat aktif
            float currentDistance = Vector3.Distance(player.position, target.position);

            if (currentDistance <= safeDistance)
            {
                TurnOff(); // Matikan otomatis jika masuk area aman
                Debug.Log("Sudah sampai di lokasi aman.");
                return;
            }

            UpdateArrowRotation();
            HandleDuration();
        }
    }

    void HandleInput()
    {
        if (Keyboard.current != null && Keyboard.current.vKey.wasPressedThisFrame)
        {
            if (isActive)
            {
                TurnOff();
            }
            else
            {
                // Cek jarak SEBELUM menyalakan
                float currentDistance = Vector3.Distance(player.position, target.position);
                if (currentDistance > safeDistance)
                {
                    TurnOn();
                }
                else
                {
                    Debug.Log("Terlalu dekat dengan target, tidak perlu petunjuk.");
                }
            }
        }
    }

    void TurnOn()
    {
        isActive = true;
        timer = activeDuration;
        if (arrowUI != null) arrowUI.gameObject.SetActive(true);
    }

    void TurnOff()
    {
        isActive = false;
        if (arrowUI != null) arrowUI.gameObject.SetActive(false);
    }

    void UpdateArrowRotation()
    {
        if (player == null || target == null || arrowUI == null) return;

        Vector3 directionToTarget = target.position - player.position;
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
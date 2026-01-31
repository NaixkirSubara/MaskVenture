using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TrailDetector : MonoBehaviour
{
    [Header("Settings")]
    //public KeyCode activationKey = KeyCode.V; // Tombol untuk menyalakan
    public bool useDuration = true;           // Apakah mati otomatis setelah sekian detik?
    public float activeDuration = 5.0f;       // Berapa lama sistem menyala (detik)

    [Header("References")]
    public Transform player;        // Karakter pemain
    public Transform target;        // Objek tujuan (Quest/Chest)
    public RectTransform arrowUI;   // UI Panah (Image) di Canvas

    // Private Variables
    private bool isActive = false;
    private float timer = 0f;

    void Start()
    {
        // Pastikan UI mati saat game mulai
        if (arrowUI != null) arrowUI.gameObject.SetActive(false);
    }

    void Update()
    {
        HandleInput();

        if (isActive)
        {
            UpdateArrowRotation();
            HandleDuration();
        }
    }

    // 1. Mengurus Input Tombol
    void HandleInput()
    {
        if (Keyboard.current != null && Keyboard.current.vKey.wasPressedThisFrame)
        {
            if (isActive)
            {
                TurnOff(); // Jika sudah nyala, matikan (Toggle)
            }
            else
            {
                TurnOn(); // Jika mati, nyalakan
            }
        }
    }

    // 2. Logika Menyalakan
    void TurnOn()
    {
        isActive = true;
        timer = activeDuration; // Reset timer
        if (arrowUI != null) arrowUI.gameObject.SetActive(true);

        // Opsional: Mainkan Sound Effect (SFX) disini
    }

    // 3. Logika Mematikan
    void TurnOff()
    {
        isActive = false;
        if (arrowUI != null) arrowUI.gameObject.SetActive(false);
    }

    // 4. Logika Hitung Arah (Inti Sistem)
    void UpdateArrowRotation()
    {
        if (player == null || target == null || arrowUI == null) return;

        // A. Hitung arah dari pemain ke target
        Vector3 directionToTarget = target.position - player.position;
        directionToTarget.y = 0; // Abaikan ketinggian agar panah tidak miring aneh

        // B. Hitung sudut antara arah hadap pemain (Forward) dan arah target
        // Kita gunakan 'SignedAngle' untuk tahu target ada di kiri (-) atau kanan (+)
        float angle = Vector3.SignedAngle(player.forward, directionToTarget, Vector3.up);

        // C. Putar UI Panah
        // Vector3(0, 0, -angle) -> Z negatif karena rotasi UI berlawanan arah jarum jam
        arrowUI.localRotation = Quaternion.Euler(0, 0, -angle);
    }

    // 5. Logika Durasi (Auto-Off ala Genshin)
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
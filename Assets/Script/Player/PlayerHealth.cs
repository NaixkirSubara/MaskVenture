using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Lives Settings")]
    public int maxLives = 3;
    public int currentLives;

    [Header("Damage Feedback")]
    public AudioClip hitSFX;

    [Header("UI")]
    public PlayerHealthUI healthUI;
    public GameObject gameOverUI; // ⬅️ UI Game Over

    [Header("Player Control")]
    public MonoBehaviour movementScript; // ⬅️ drag script movement ke sini

    private bool isDead = false;

    void Start()
    {
        currentLives = maxLives;

        if (healthUI != null)
            healthUI.UpdateLife(currentLives);

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    public void TakeDamage()
    {
        if (isDead) return;

        currentLives--;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);

        // 🔊 SFX kena hit
        if (AudioManager.Instance != null && hitSFX != null)
        {
            AudioManager.Instance.PlaySFX(hitSFX);
        }

        // Update UI
        if (healthUI != null)
            healthUI.UpdateLife(currentLives);

        if (currentLives <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;

        Debug.Log("Player Mati");

        // 🛑 Matikan movement
        if (movementScript != null)
            movementScript.enabled = false;

        // 🖥️ Tampilkan Game Over UI
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        // (opsional) unlock cursor kalau FPS
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}

using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Lives Settings")]
    public int maxLives = 3;
    public int currentLives;

    [Header("Damage Feedback")]
    public AudioSource audioSource;
    public AudioClip hitSFX;

    [Header("UI")]
    public PlayerHealthUI healthUI; // <--- UI reference

    private bool isDead = false;

    void Start()
    {
        currentLives = maxLives;
        if (healthUI != null)
            healthUI.UpdateLife(currentLives); // Update UI awal
    }

    public void TakeDamage()
    {
        if (isDead) return;

        currentLives -= 1;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);

        // 🔊 Sound kena damage
        if (audioSource != null && hitSFX != null)
        {
            audioSource.PlayOneShot(hitSFX);
        }

        // Update UI
        if (healthUI != null)
            healthUI.UpdateLife(currentLives);

        Debug.Log("Player Nyawa: " + currentLives);

        if (currentLives <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player Mati");
        // Contoh: disable movement, tampilkan GameOver UI, dll
    }
}

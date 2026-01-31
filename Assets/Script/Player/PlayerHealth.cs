using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Damage Feedback")]
    public AudioSource audioSource;
    public AudioClip hitSFX;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // 🔊 Sound kena damage
        if (audioSource != null && hitSFX != null)
        {
            audioSource.PlayOneShot(hitSFX);
        }

        Debug.Log("Player HP: " + currentHealth);

        // ☠️ Mati
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player Mati");

        // Contoh opsi:
        // Time.timeScale = 0f;
        // tampilkan Game Over UI
        // disable movement script
    }
}

using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Lives Settings")]
    public int maxLives = 3;
    public int currentLives;

    [Header("Damage Feedback")]
    public AudioClip hitSFX;   // ⬅️ cukup AudioClip saja

    [Header("UI")]
    public PlayerHealthUI healthUI;

    private bool isDead = false;

    void Start()
    {
        currentLives = maxLives;

        if (healthUI != null)
            healthUI.UpdateLife(currentLives);
    }

    public void TakeDamage()
    {
        if (isDead) return;

        currentLives--;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);

        // Play SFX lewat AudioManager
        if (AudioManager.Instance != null && hitSFX != null)
        {
            AudioManager.Instance.PlaySFX(hitSFX);
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
        // disable movement / GameOver UI
    }
}

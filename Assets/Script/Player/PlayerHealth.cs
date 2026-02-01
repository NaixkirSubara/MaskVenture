using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Lives Settings")]
    public int maxLives = 3;
    public int currentLives;

    [Header("Damage Feedback")]
    public AudioClip hitSFX;
    public AudioClip deathSFX; //  SFX saat mati

    [Header("UI")]
    public PlayerHealthUI healthUI;
    public GameObject gameOverUI;

    [Header("Player Control")]
    public MonoBehaviour movementScript;

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

        //  SFX kena hit
        if (AudioManager.Instance != null && hitSFX != null)
        {
            AudioManager.Instance.PlaySFX(hitSFX);
        }

        if (healthUI != null)
            healthUI.UpdateLife(currentLives);

        if (currentLives <= 0)
            Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player Mati");

        //  SFX mati
        if (AudioManager.Instance != null && deathSFX != null)
        {
            AudioManager.Instance.PlaySFX(deathSFX);
        }

        //  Matikan movement
        if (movementScript != null)
            movementScript.enabled = false;

        // 🖥️ Game Over UI
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        // 🖱️ Unlock cursor (FPS)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}

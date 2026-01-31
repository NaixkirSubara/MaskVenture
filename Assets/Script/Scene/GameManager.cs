using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Input System

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public GameObject pauseUI;

    [Header("Player Control")]
    public MonoBehaviour movementScript;

    private bool isPaused = false;

    // ================= SINGLETON =================
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ================= SCENE LOAD =================
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // RESET STATE
        Time.timeScale = 1f;
        isPaused = false;

        // Cari ulang Pause UI
        GameObject foundPauseUI = GameObject.FindWithTag("PauseUI");
        if (foundPauseUI != null)
        {
            pauseUI = foundPauseUI;
            pauseUI.SetActive(false);
        }

        // Cari ulang movement player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // GANTI dengan script movement kamu
            movementScript = player.GetComponent<MonoBehaviour>();
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // ================= UPDATE =================
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    // ================= PAUSE =================
    public void Pause()
    {
        if (isPaused) return;

        isPaused = true;
        Time.timeScale = 0f;

        if (pauseUI != null)
            pauseUI.SetActive(true);

        if (movementScript != null)
            movementScript.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        if (!isPaused) return;

        isPaused = false;
        Time.timeScale = 1f;

        if (pauseUI != null)
            pauseUI.SetActive(false);

        if (movementScript != null)
            movementScript.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // ================= SCENE CONTROL =================
    public void RestartScene()
    {
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;

        SceneManager.LoadScene("MainMenu");
    }

    public void LoadSceneByName(string sceneName)
    {
        Time.timeScale = 1f;
        isPaused = false;

        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}

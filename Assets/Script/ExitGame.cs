using UnityEngine;

public class ExitGame : MonoBehaviour
{
    // Panggil dari Button UI
    public void QuitGame()
    {
        Debug.Log("Quit Game");

        // Untuk build
        Application.Quit();

        // Untuk test di Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

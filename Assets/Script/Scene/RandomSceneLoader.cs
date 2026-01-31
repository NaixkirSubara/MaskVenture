using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomSceneLoader : MonoBehaviour
{
    [Header("Scenes")]
    [Tooltip("Isi nama scene, HARUS sama persis dengan di Build Settings")]
    public string[] sceneNames;

    public void LoadRandomScene()
    {
        if (sceneNames == null || sceneNames.Length == 0)
        {
            Debug.LogWarning("Scene list kosong!");
            return;
        }

        int randomIndex = Random.Range(0, sceneNames.Length);
        SceneManager.LoadScene(sceneNames[randomIndex]);
    }
}


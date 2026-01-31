using UnityEngine;
using UnityEngine.SceneManagement; // Penting untuk perpindahan scene

public class SceneTrigger : MonoBehaviour
{
    [Header("Pengaturan Scene")]
    [SerializeField] private string sceneName; // Nama scene tujuan

    // Fungsi ini dipanggil otomatis saat objek lain masuk ke area trigger
    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah yang menyentuh trigger adalah Player
        if (other.CompareTag("Player"))
        {
            // Pindah ke scene berdasarkan nama yang diisi di Inspector
            SceneManager.LoadScene(sceneName);
        }
    }
}
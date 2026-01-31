using UnityEngine;

public class GlobalAbilityManager : MonoBehaviour
{
    public static GlobalAbilityManager Instance;

    // Status apakah ada skill yang sedang aktif
    public bool IsAnyAbilityActive { get; private set; } = false;

    void Awake()
    {
        // Setup Singleton sederhana
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Fungsi untuk meminta izin mengaktifkan skill
    public bool TryActivateAbility()
    {
        if (IsAnyAbilityActive)
        {
            Debug.Log("GAGAL: Skill lain sedang aktif!");
            return false; // Ditolak
        }

        IsAnyAbilityActive = true; // Kunci sistem
        return true; // Diizinkan
    }

    // Fungsi untuk melepas kunci setelah skill selesai
    public void FinishAbility()
    {
        IsAnyAbilityActive = false;
        Debug.Log("Sistem Bebas: Skill lain bisa digunakan sekarang.");
    }
}
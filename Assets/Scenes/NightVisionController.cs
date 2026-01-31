using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class NightVisionController : MonoBehaviour
{
    // --- TAMBAHAN BARU ---
    [Header("Settings")]
    // Drag & Drop Directional Light Anda ke slot ini di Inspector
    public Light directionalLight;
    // ---------------------

    private List<GameObject> hiddenObjects = new List<GameObject>();
    private bool isNightVisionActive = false;

    void Start()
    {
        // 1. Logika Mencari Hidden Objects
        GameObject[] targets = GameObject.FindGameObjectsWithTag("HiddenObject");

        foreach (GameObject go in targets)
        {
            hiddenObjects.Add(go);
            // Default: Hidden Objects tidak terlihat
            go.SetActive(false);
        }

        // 2. Logika Directional Light Awal
        // Pastikan lampu menyala saat awal game
        if (directionalLight != null)
        {
            directionalLight.enabled = true;
        }
        else
        {
            Debug.LogWarning("Directional Light belum dimasukkan ke script di Inspector!");
        }
    }

    void Update()
    {
        // Catatan: Script Anda menggunakan 'qKey'. 
        // Jika ingin sesuai komentar (tombol N), ganti 'qKey' menjadi 'nKey'.
        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
        {
            ToggleNightVision();
        }
    }

    void ToggleNightVision()
    {
        // Ubah status true/false
        isNightVisionActive = !isNightVisionActive;

        // --- UPDATE OBJEK RAHASIA ---
        foreach (GameObject go in hiddenObjects)
        {
            if (go != null)
            {
                // Jika NV Aktif -> Objek Muncul
                go.SetActive(isNightVisionActive);
            }
        }

        // --- UPDATE DIRECTIONAL LIGHT ---
        if (directionalLight != null)
        {
            // Logika terbalik:
            // Jika NV Aktif (true) -> Lampu Mati (false)
            // Jika NV Mati (false) -> Lampu Nyala (true)
            directionalLight.enabled = !isNightVisionActive;
        }
    }
}
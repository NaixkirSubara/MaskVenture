using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class NightVisionController : MonoBehaviour
{
    [Header("Settings")]
    public Light directionalLight;
    public float activeDuration = 10f; // Durasi menyala setiap kali aktif
    public float cooldownTime = 5f;    // Waktu tunggu sebelum bisa dipakai lagi

    [Header("Status (Read Only)")]
    public bool isNightVisionActive = false;
    public bool isReadyToUse = true;
    private float timer = 0f;

    // --- VARIABEL INTERNAL ---
    private List<GameObject> hiddenObjects = new List<GameObject>();
    private Material defaultSkybox;
    private Color defaultAmbientColor;

    void Start()
    {
        defaultSkybox = RenderSettings.skybox;
        defaultAmbientColor = RenderSettings.ambientLight;

        GameObject[] targets = GameObject.FindGameObjectsWithTag("HiddenObject");
        foreach (GameObject go in targets)
        {
            hiddenObjects.Add(go);
            go.SetActive(false);
        }
    }

    void Update()
    {
        // 1. Input untuk Mengaktifkan (Hanya bisa jika Ready)
        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
        {
            if (!isNightVisionActive && isReadyToUse)
            {
                ActivateNightVision();
            }
            else if (isNightVisionActive)
            {
                // Jika ingin bisa mematikan manual sebelum waktu habis
                DeactivateNightVision();
            }
        }

        // 2. Logika Hitung Mundur saat Aktif
        if (isNightVisionActive)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                DeactivateNightVision();
            }
        }

        // 3. Logika Cooldown (Reset Kembali)
        if (!isNightVisionActive && !isReadyToUse)
        {
            timer += Time.deltaTime;
            if (timer >= cooldownTime)
            {
                isReadyToUse = true;
                timer = 0; // Reset timer untuk penggunaan berikutnya
                Debug.Log("Night Vision Ready!");
            }
        }
    }

    void ActivateNightVision()
    {
        isNightVisionActive = true;
        isReadyToUse = false;
        timer = activeDuration; // <--- RESET WAKTU KE PENUH
        ApplyVisuals(true);
    }

    void DeactivateNightVision()
    {
        isNightVisionActive = false;
        timer = 0; // Mulai hitung cooldown dari 0
        ApplyVisuals(false);
    }

    void ApplyVisuals(bool state)
    {
        // Toggle Objek Rahasia
        foreach (GameObject go in hiddenObjects)
        {
            if (go != null) go.SetActive(state);
        }

        // Toggle Directional Light
        if (directionalLight != null)
        {
            directionalLight.enabled = !state;
        }

        // Toggle Skybox & Ambient
        if (state)
        {
            RenderSettings.skybox = null;
            RenderSettings.ambientLight = Color.black;
        }
        else
        {
            RenderSettings.skybox = defaultSkybox;
            RenderSettings.ambientLight = defaultAmbientColor;
        }

        DynamicGI.UpdateEnvironment();
    }
}
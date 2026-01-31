using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class NightVisionController : MonoBehaviour
{
    [Header("Settings")]
    public Light directionalLight;
    public float activeDuration = 10f;
    public float cooldownTime = 5f;

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
        // 1. Input untuk Mengaktifkan
        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
        {
            if (!isNightVisionActive && isReadyToUse)
            {
                // INTEGRASI: Minta izin ke Manager dulu
                if (GlobalAbilityManager.Instance.TryActivateAbility())
                {
                    ActivateNightVision();
                }
            }
            else if (isNightVisionActive)
            {
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

        // 3. Logika Cooldown
        if (!isNightVisionActive && !isReadyToUse)
        {
            timer += Time.deltaTime;
            if (timer >= cooldownTime)
            {
                isReadyToUse = true;
                timer = 0;
                Debug.Log("Night Vision Ready!");
            }
        }
    }

    void ActivateNightVision()
    {
        isNightVisionActive = true;
        isReadyToUse = false;
        timer = activeDuration;
        ApplyVisuals(true);
    }

    void DeactivateNightVision()
    {
        // Cek agar tidak memanggil FinishAbility berkali-kali jika sudah mati
        if (!isNightVisionActive) return;

        isNightVisionActive = false;
        timer = 0;
        ApplyVisuals(false);

        // INTEGRASI: Lapor ke Manager bahwa skill sudah selesai
        GlobalAbilityManager.Instance.FinishAbility();
    }

    void ApplyVisuals(bool state)
    {
        foreach (GameObject go in hiddenObjects)
        {
            if (go != null) go.SetActive(state);
        }

        if (directionalLight != null) directionalLight.enabled = !state;

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
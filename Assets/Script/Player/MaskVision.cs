using UnityEngine;
using UnityEngine.InputSystem;

public class MaskVision : MonoBehaviour
{
    [Header("References")]
    public GameObject maskObject;       // Model topeng
    public GameObject lineArtEffect;    // GameObject efek line art (PostProcess / Script)

    [Header("Settings")]
    public Key toggleKey = Key.Digit1;

    private bool isMaskOn = false;

    void Start()
    {
        // Pastikan awalnya mati
        if (maskObject != null) maskObject.SetActive(false);
        if (lineArtEffect != null) lineArtEffect.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            ToggleMask();
        }
    }

    void ToggleMask()
    {
        isMaskOn = !isMaskOn;

        if (maskObject != null)
            maskObject.SetActive(isMaskOn);

        if (lineArtEffect != null)
            lineArtEffect.SetActive(isMaskOn);

        Debug.Log(isMaskOn ? "[MASK] Dipakai" : "[MASK] Dilepas");
    }
}

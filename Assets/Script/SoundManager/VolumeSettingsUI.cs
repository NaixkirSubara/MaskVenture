using UnityEngine;
using UnityEngine.UI;

public class VolumeSettingsUI : MonoBehaviour
{
    [Header("UI References")]
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;

    // Gunakan OnEnable, bukan Start.
    // Agar setiap kali panel menu dibuka/aktif, slider langsung menyesuaikan posisi.
    void OnEnable() 
    {
        // Cek apakah AudioManager ada. Jika tidak, script berhenti untuk mencegah Error.
        if (AudioManager.Instance == null)
        {
            Debug.LogError("AudioManager belum ada di Scene ini! Pastikan start dari Main Menu.");
            return;
        }

        SetupSlider(masterSlider, AudioManager.Instance.GetMasterVolume(), AudioManager.Instance.SetMasterVolume);
        SetupSlider(sfxSlider,    AudioManager.Instance.GetSFXVolume(),    AudioManager.Instance.SetSFXVolume);
        SetupSlider(musicSlider,  AudioManager.Instance.GetMusicVolume(),  AudioManager.Instance.SetMusicVolume);
    }

    private void SetupSlider(Slider slider, float currentVolume, UnityEngine.Events.UnityAction<float> onValueChangedAction)
    {
        if (slider == null) return;

        // 1. Hapus listener lama dulu supaya tidak menumpuk (double call)
        slider.onValueChanged.RemoveAllListeners();

        // 2. Set posisi visual slider sesuai data yang tersimpan
        slider.value = currentVolume;

        // 3. Baru pasang listener untuk mendeteksi perubahan
        slider.onValueChanged.AddListener(onValueChangedAction);
    }
}
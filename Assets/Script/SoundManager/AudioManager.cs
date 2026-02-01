using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement; // Wajib ada untuk deteksi pindah scene

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    // PlayerPrefs Keys
    private const string MASTER_KEY = "MasterVolume";
    private const string MUSIC_KEY  = "MusicVolume";
    private const string SFX_KEY    = "SFXVolume";

    // Exposed parameter names (SESUAIKAN DENGAN MIXER ANDA)
    private const string MASTER_PARAM = "MyExposedParam";
    private const string MUSIC_PARAM  = "MyExposedParam 1";
    private const string SFX_PARAM    = "MyExposedParam 2";

    void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadVolume();
    }

    // ================= BAGIAN BARU (SOLUSI OPSI 1) =================
    // Fungsi ini mendaftarkan deteksi perpindahan scene
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Fungsi ini melepas deteksi saat script mati (mencegah error)
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Fungsi ini OTOMATIS dipanggil Unity setiap kali scene selesai loading
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Setiap masuk scene baru, MATIKAN MUSIK DULU
        StopMusic();
    }
    // ===============================================================

    // ================= MUSIC =================
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;

        // Cek agar tidak me-restart lagu jika lagunya sama
        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
        musicSource.clip = null;
    }

    // ================= SFX =================
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    // ================= VOLUME =================
    public void SetMasterVolume(float value)
    {
        SetVolume(MASTER_KEY, MASTER_PARAM, value);
    }

    public void SetMusicVolume(float value)
    {
        SetVolume(MUSIC_KEY, MUSIC_PARAM, value);
    }

    public void SetSFXVolume(float value)
    {
        SetVolume(SFX_KEY, SFX_PARAM, value);
    }

    private void SetVolume(string prefsKey, string mixerParam, float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        // Mengubah value 0-1 menjadi decibel logaritmik
        audioMixer.SetFloat(mixerParam, Mathf.Log10(value) * 20f);
        
        PlayerPrefs.SetFloat(prefsKey, value);
        PlayerPrefs.Save();
    }

    // ================= LOAD =================
    private void LoadVolume()
    {
        SetVolume(MASTER_KEY, MASTER_PARAM, PlayerPrefs.GetFloat(MASTER_KEY, 1f));
        SetVolume(MUSIC_KEY,  MUSIC_PARAM,  PlayerPrefs.GetFloat(MUSIC_KEY, 1f));
        SetVolume(SFX_KEY,    SFX_PARAM,    PlayerPrefs.GetFloat(SFX_KEY, 1f));
    }

    // ================= GET =================
    public float GetMasterVolume() => PlayerPrefs.GetFloat(MASTER_KEY, 1f);
    public float GetMusicVolume()  => PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
    public float GetSFXVolume()    => PlayerPrefs.GetFloat(SFX_KEY, 1f);
}
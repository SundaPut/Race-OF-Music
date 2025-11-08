using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Base Audio Configuration")]
    public Sound[] musicSounds;
    public Sound[] sfxSounds;
    // AudioSource internal (hanya sebagai cadangan/untuk PlayMusic/PlayRandomMusic)
    public AudioSource musicSource, sfxSource;

    // --- KONTROL VOLUME & MUSIK RANDOM ---
    [Header("Music Volume Control & Random Tracks")]
    [Range(0f, 1f)] public float initialGameVolume = 0.7f;
    [Range(0f, 1f)] public float volumeIncreaseOnHit = 0.15f;

    public Sound[] randomizableMusic;

    private const string MAIN_MENU_SCENE_NAME = "MainMenu";
    private AudioSource audioSource;

    // VARIABEL KHUSUS: Untuk mengontrol AudioSource dari objek Music.cs
    private AudioSource externalMusicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        SetupExternalMusicSource();

    }

    private void SetupExternalMusicSource()
    {
        // Cari instance Music.cs
        if (Music.instance != null)
        {
            externalMusicSource = Music.instance.GetComponent<AudioSource>();
        }

        InitializeVolume();

        if (Music.instance != null)
        {
            Music.instance.MusicStop(); 
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Selalu set up ulang referensi (jika objek MusicManager ada)
        SetupExternalMusicSource();

        // Inisialisasi volume
        InitializeVolume();

        // Putar musik yang benar
        if (scene.name == MAIN_MENU_SCENE_NAME)
        {
            PlayMusic("Ambient");
        }
        else
        {
            PlayRandomMusic();
        }
    }

    // --- FUNGSI UTILITY VOLUME ---

    private void InitializeVolume()
    {
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", initialGameVolume);
        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);

        // 1. Atur volume AudioSource internal
        musicSource.volume = savedMusicVolume;
        sfxSource.volume = savedSFXVolume;

        // 2. Atur volume AudioSource eksternal (Music.cs)
        if (externalMusicSource != null)
        {
            externalMusicSource.volume = savedMusicVolume;
        }
    }

    // --- FUNGSI KONTROL VOLUME GAME (Dipanggil oleh GameManager) ---

    public void IncreaseVolumeOnDamage()
    {
        float newVolume = Mathf.Min(musicSource.volume + volumeIncreaseOnHit, 1.0f);

        // Naikkan volume internal AudioManager
        musicSource.volume = newVolume;

        // Naikkan volume pada sumber eksternal (Music.cs)
        if (externalMusicSource != null)
        {
            externalMusicSource.volume = newVolume;
        }
    }

    public void ResetVolume()
    {
        InitializeVolume();
    }

    public void PlayRandomMusic()
    {
        musicSource.Stop();
        if (externalMusicSource != null) externalMusicSource.Stop();

        if (randomizableMusic.Length == 0) return;

        int randomIndex = Random.Range(0, randomizableMusic.Length);
        Sound randomSound = randomizableMusic[randomIndex];

        musicSource.clip = randomSound.clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayMusic(string name)
    {
        musicSource.Stop();
        if (externalMusicSource != null) externalMusicSource.Stop();

        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null) return;

        musicSource.clip = s.clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void SetMusicVolume(float volume)
    {
        // Atur volume internal
        musicSource.volume = volume;

        // Atur volume eksternal
        if (externalMusicSource != null)
        {
            externalMusicSource.volume = volume;
        }

        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null) return;
        sfxSource.PlayOneShot(s.clip);
    }

    public void MusicStop()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
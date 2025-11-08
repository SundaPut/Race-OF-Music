using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeBarUI : MonoBehaviour
{
    [Header("UI References")]
    public Slider volumeSlider;
    [Tooltip("List semua Image yang mewakili bar volume (misal: 12 bar)")]
    public List<Image> volumeBars;

    [Header("Configuration")]
    public bool isMusicVolume = true;

    private string VolumePrefKey => isMusicVolume ? "MusicVolume" : "SFXVolume";

    private int totalBars;

    private void Start()
    {
        totalBars = volumeBars.Count;

        if (volumeSlider == null)
        {
            Debug.LogError("Volume Slider tidak terpasang di " + gameObject.name);
            return;
        }

        // --- 1. Memuat Nilai Tersimpan ---
        LoadSavedVolume();

        // Tambahkan listener setelah nilai dimuat
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        // --- 2. Perbarui Visual Bar Berdasarkan Nilai yang Dimuat ---
        UpdateBarVisuals(volumeSlider.value);
    }

    private void LoadSavedVolume()
    {
        // Mendapatkan nilai tersimpan, defaultnya adalah 1.0f (volume penuh)
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 1.0f);

        // Mengatur nilai slider
        volumeSlider.value = savedVolume;

        // Mengatur volume di AudioManager saat game dimulai
        if (AudioManager.Instance != null)
        {
            if (isMusicVolume)
            {
                AudioManager.Instance.SetMusicVolume(savedVolume);
            }
            else
            {
                AudioManager.Instance.SetSFXVolume(savedVolume);
            }
        }
    }


    private void OnVolumeChanged(float value)
    {
        // 1. Terapkan Volume dan Simpan Nilai ke PlayerPrefs

        if (AudioManager.Instance != null)
        {
            if (isMusicVolume)
            {
                AudioManager.Instance.SetMusicVolume(value);
            }
            else
            {
                AudioManager.Instance.SetSFXVolume(value);
            }
        }

        // --- Menyimpan Nilai Setiap Kali Slider Berubah ---
        PlayerPrefs.SetFloat(VolumePrefKey, value);
        PlayerPrefs.Save(); // Penting: Simpan perubahan ke disk

        // 2. Perbarui Tampilan Visual
        UpdateBarVisuals(value);
    }

    private void UpdateBarVisuals(float volume)
    {
        int barsToFill = Mathf.CeilToInt(volume * totalBars);

        for (int i = 0; i < totalBars; i++)
        {
            if (i < barsToFill)
            {
                // Bar aktif/terisi (Sesuai permintaan: Biru)
                volumeBars[i].color = Color.blue;
            }
            else
            {
                // Bar tidak aktif (Default sebelumnya: Putih. Diubah menjadi Biru Pudar untuk kontras)
                // Anda bisa ganti Color.gray, Color.black, atau Color.white
                volumeBars[i].color = Color.gray;
            }
        }
    }
}

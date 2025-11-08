using UnityEngine;

// Skrip ini akan memutar musik secara acak saat game dimulai
// dan akan memastikan musik tidak berhenti saat scene di-restart.
//
// CARA PENGGUNAAN:
// 1. Buat sebuah GameObject kosong di scene utama Anda, beri nama "MusicManager".
// 2. Tempelkan (attach) skrip ini ke GameObject "MusicManager" tersebut.
// 3. Di Inspector, Anda akan melihat array "Music Tracks". Atur ukurannya
//    sesuai jumlah lagu yang Anda punya.
// 4. Seret (drag & drop) semua file musik Anda dari folder Project ke dalam
//    slot-slot di array "Music Tracks".

[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour
{
    // Daftar semua trek musik yang bisa diputar
    public AudioClip[] musicTracks;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float initialVolume = 0.7f; // Volume awal musik
    [Range(0f, 1f)] public float volumeIncreaseOnHit = 0.15f; // Kenaikan volume saat terkena obstacle

    private AudioSource audioSource;

    // Singleton pattern untuk memastikan hanya ada satu Music Manager
    public static Music instance;

    void Awake()
    {
        // Cek apakah sudah ada instance Music Manager
        if (instance == null)
        {
            // Jika belum ada, jadikan ini sebagai instance utama
            instance = this;
            // Jangan hancurkan objek ini saat berpindah scene
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Jika sudah ada, hancurkan duplikat ini
            Destroy(gameObject);
            return; // Hentikan eksekusi lebih lanjut
        }

        // Ambil komponen AudioSource yang terpasang
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        // Pastikan ada musik di dalam daftar
        if (musicTracks.Length > 0)
        {
            // Pilih musik secara acak dari daftar
            ResetVolume(); // Atur volume ke nilai awal
            audioSource.clip = musicTracks[Random.Range(0, musicTracks.Length)];
            audioSource.loop = true; // Atur agar musik berulang
            audioSource.Play(); // Mulai mainkan musik
        }
    }

    /// <summary>
    /// Menaikkan volume musik, dipanggil saat player terkena damage.
    /// </summary>
    public void IncreaseVolume()
    {
        // Naikkan volume, tapi batasi agar tidak lebih dari 1.0f
        audioSource.volume = Mathf.Min(audioSource.volume + volumeIncreaseOnHit, 1.0f);
    }

    /// <summary>
    /// Mengembalikan volume ke pengaturan awal.
    /// </summary>
    public void ResetVolume()
    {
        if (audioSource != null) audioSource.volume = initialVolume;
    }
}

    
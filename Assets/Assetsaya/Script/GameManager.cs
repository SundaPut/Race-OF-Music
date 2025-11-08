using UnityEngine;
using UnityEngine.UI; // Diperlukan untuk mengakses komponen UI
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // Diperlukan untuk me-restart scene

public class GameManager : MonoBehaviour
{
    // --- Singleton Setup ---
    public static GameManager Instance;

    [Header("Game References")]
    public GameObject player;
    public GameObject mountainObject;
    [Tooltip("Seret objek dengan script ScoreManager di sini.")]
    public ScoreManager scoreManager;

    // VARIABEL UI GAME OVER DIPINDAHKAN KE SINI DARI SCCOREMANAGER
    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public List<Image> finalScoreDisplayImages;
    // public Text scoreText; // DIHAPUS: Tidak lagi menggunakan Text UI

    [Header("Game Speed Settings")]
    public float initialGameSpeed = 5f;
    public float speedIncreasePerScore = 0.1f;
    public float currentGameSpeed { get; private set; }

    // private int score = 0; // DIHAPUS: Skor sekarang dikelola oleh ScoreManager
    private bool isGameOver = false;
    private int playerHealth = 2;
    private float screenWidth;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Cari ScoreManager jika belum diseret di Inspector
        if (scoreManager == null)
        {
            scoreManager = FindObjectOfType<ScoreManager>();
        }
    }

    void Start()
    {
        Time.timeScale = 1f;
        currentGameSpeed = initialGameSpeed;

        // Nonaktifkan panel kalah di awal
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ResetVolume();
        }

        screenWidth = Camera.main.orthographicSize * Camera.main.aspect;

        if (mountainObject != null)
        {
            mountainObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isGameOver)
        {
            return;
        }
    }

    // --- FUNGSI SKOR (Menggunakan ScoreManager) ---
    public void AddScore(int amount)
    {
        if (isGameOver || scoreManager == null) return;

        // 1. Tambah skor di ScoreManager
        scoreManager.AddScore(amount);

        // 2. Tingkatkan kecepatan
        int currentScoreFromManager = scoreManager.GetCurrentScore();
        currentGameSpeed = initialGameSpeed + (currentScoreFromManager * speedIncreasePerScore);
        Debug.Log("New Game Speed: " + currentGameSpeed);
    }


    public void PlayerTookDamage()
    {
        if (isGameOver) return;

        Debug.Log("GameManager: PlayerTookDamage() dipanggil!");
        playerHealth--;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.IncreaseVolumeOnDamage();
            AudioManager.Instance.PlaySFX("Tabrakan");
        }

        if (playerHealth == 1)
        {
            if (mountainObject != null)
            {
                Debug.Log("GameManager: Nyawa sisa 1, mencoba mengaktifkan gunung.");
                mountainObject.SetActive(true);
            }
        }
        else if (playerHealth <= 0)
        {
            StartCoroutine(MountainEatsPlayerSequence());
        }
    }

    private IEnumerator MountainEatsPlayerSequence()
    {
        isGameOver = true;

        if (player != null)
        {
            // Nonaktifkan kontrol pemain
            Component playerMove = player.GetComponent("PlayerMove"); // Cari komponen PlayerMove
            if (playerMove != null)
            {
                ((MonoBehaviour)playerMove).enabled = false;
            }
        }

        // Animasi gunung membesar (tetap sama)
        if (mountainObject != null)
        {
            float duration = 1.5f;
            float timer = 0;
            Vector3 startScale = mountainObject.transform.localScale;
            Vector3 endScale = startScale * 3f;

            while (timer < duration)
            {
                mountainObject.transform.localScale = Vector3.Lerp(startScale, endScale, timer / duration);
                timer += Time.deltaTime;
                AudioManager.Instance?.PlaySFX("Bebatuan");

                yield return null;
            }
        }

        HandleGameOver();
    }

    // --- FUNGSI UTAMA GAME OVER ---
    public void HandleGameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (scoreManager != null)
            {
                scoreManager.DisplayFinalScore(scoreManager.GetCurrentScore(), finalScoreDisplayImages);
            }
        }

        // Hancurkan pemain
        if (player != null)
        {
            Destroy(player);
        }
        AudioManager.Instance?.ResetVolume();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        AudioManager.Instance?.PlayRandomMusic();

    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        AudioManager.Instance?.MusicStop();
    }
}

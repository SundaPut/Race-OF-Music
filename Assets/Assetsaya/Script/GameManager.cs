using UnityEngine;
using UnityEngine.UI; // Diperlukan untuk mengakses komponen UI
using System.Collections; // Diperlukan untuk Coroutine
using UnityEngine.SceneManagement; // Diperlukan untuk me-restart scene

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject mountainObject; // Seret objek Mountain dari Hierarchy ke sini
    public Text scoreText; // Seret komponen UI Text ke sini di Inspector

    private int score = 0;
    private bool isGameOver = false;
    private int playerHealth = 2; // Pemain punya 2 nyawa
    private float screenWidth;

    void Start()
    {
        // Hitung lebar layar dalam satuan world unit
        screenWidth = Camera.main.orthographicSize * Camera.main.aspect;

        // Pastikan gunung tidak terlihat di awal permainan
        if (mountainObject != null)
        {
            mountainObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isGameOver)
        {
            // Jika game over, cek input untuk restart
            if (Input.GetKeyDown(KeyCode.R))
            {
                // Muat ulang scene saat ini
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            return; // Hentikan update jika game sudah berakhir
        }

        // Skor sekarang ditangani oleh AddScore()
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return; // Jangan tambah skor jika sudah game over
        score += amount;
        if (scoreText != null) scoreText.text = "Score: " + score;
    }

    public void PlayerTookDamage()
    {
        Debug.Log("GameManager: PlayerTookDamage() dipanggil!");
        playerHealth--; // Kurangi nyawa pemain

        if (playerHealth == 1)
        {
            // Jika ini adalah pukulan pertama, munculkan gunung
            if (mountainObject != null)
            {
                Debug.Log("GameManager: Nyawa sisa 1, mencoba mengaktifkan gunung.");
                mountainObject.SetActive(true);
            }
        }
        else if (playerHealth <= 0)
        {
            // Jika nyawa habis, mulai urutan "gunung memakan pemain"
            StartCoroutine(MountainEatsPlayerSequence());
        }
    }

    private IEnumerator MountainEatsPlayerSequence()
    {
        isGameOver = true; // Hentikan permainan (spawn batu, skor, dll)

        // Nonaktifkan kontrol pemain
        if (player != null)
        {
            player.GetComponent<PlayerMove>().enabled = false;
        }

        // Animasi gunung membesar
        if (mountainObject != null)
        {
            float duration = 1.5f; // Durasi animasi dalam detik
            float timer = 0;
            Vector3 startScale = mountainObject.transform.localScale;
            Vector3 endScale = startScale * 3f; // Buat skala 3x lebih besar

            while (timer < duration)
            {
                mountainObject.transform.localScale = Vector3.Lerp(startScale, endScale, timer / duration);
                timer += Time.deltaTime;
                yield return null; // Tunggu frame berikutnya
            }
        }

        // Setelah gunung membesar, panggil GameOver
        GameOver();
    }

    public void GameOver()
    {
        isGameOver = true;
        Destroy(player); // Hancurkan player
        if (scoreText != null) scoreText.text = "GAME OVER\nScore: " + score + "\nTekan 'R' untuk Restart";
    }
}

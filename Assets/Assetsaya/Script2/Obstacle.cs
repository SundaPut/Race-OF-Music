using UnityEngine;

// Skrip ini mengatur perilaku rintangan (obstacle)
public class Obstacle : MonoBehaviour
{
    private bool hasScored = false;
    private Transform playerTransform;

    // Ganti FindObjectOfType dengan Singleton Instance
    private GameManager gameManagerInstance;

    // Batas aman: Obstacle harus 1.0f unit LEBIH RENDAH dari posisi pemain untuk skor bertambah.
    // Sesuaikan nilai ini jika perlu.
    private const float SCORE_BOUNDARY_OFFSET = 1.0f;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }

        // Dapatkan Singleton GameManager
        gameManagerInstance = GameManager.Instance;

        if (playerTransform == null || gameManagerInstance == null)
        {
            Destroy(gameObject); // Hancurkan jika setup gagal
        }
    }

    void Update()
    {
        if (gameManagerInstance == null) return;

        // Gerakkan obstacle
        transform.Translate(Vector2.down * gameManagerInstance.currentGameSpeed * Time.deltaTime);

        // --- Logika Penambahan Skor ---
        if (!hasScored && playerTransform != null)
        {
            // Skor bertambah jika posisi Y rintangan lebih rendah dari posisi Y pemain dikurangi offset.
            // Contoh: Posisi pemain Y=0. Skor bertambah saat rintangan mencapai Y < -1.0f
            if (transform.position.y < playerTransform.position.y - SCORE_BOUNDARY_OFFSET)
            {
                gameManagerInstance.AddScore(1); // Panggil AddScore di GameManager
                hasScored = true;
            }
        }

        // Hapus obstacle jika sudah terlalu jauh di bawah layar
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Panggil PlayerTookDamage di GameManager saat bertabrakan
        if (other.CompareTag("Player") && gameManagerInstance != null)
        {
            gameManagerInstance.PlayerTookDamage();
            Destroy(gameObject);
        }
    }
}
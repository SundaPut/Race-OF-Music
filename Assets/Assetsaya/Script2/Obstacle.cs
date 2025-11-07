using UnityEngine;

// Skrip ini mengatur perilaku rintangan (obstacle)
public class Obstacle : MonoBehaviour
{ 
    
    private bool hasScored = false; // Untuk memastikan skor hanya ditambah sekali
    private Transform playerTransform;
    private GameManager gameManager;

    void Start()
    {
        // Cari objek Player dan GameManager saat obstacle dibuat
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        // Gerakkan obstacle lurus ke bawah
        if (gameManager != null)
        {
            transform.Translate(Vector2.down * gameManager.currentGameSpeed * Time.deltaTime);
        }

        // Jika obstacle melewati pemain dan belum memberikan skor, tambahkan skor
        if (!hasScored && playerTransform != null && transform.position.y < playerTransform.position.y) {
            if (gameManager != null) gameManager.AddScore(1);
            hasScored = true; // Tandai bahwa skor sudah diberikan
        }

        // Hapus obstacle jika sudah terlalu jauh di bawah layar
        if (transform.position.y < -8f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Jika rintangan mengenai player, panggil GameOver
        if (other.CompareTag("Player"))
        {
            Debug.Log("Obstacle: Bertabrakan dengan Player!");
            // Beri tahu GameManager bahwa pemain terkena damage
            if (gameManager != null) gameManager.PlayerTookDamage();
            
            Destroy(gameObject); // Hancurkan rintangan ini
        }
    }
}
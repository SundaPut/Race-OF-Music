using UnityEngine;

// Skrip ini mengatur perilaku rintangan (obstacle)
public class Obstacle : MonoBehaviour
{
    public float speed = 5f; // Kecepatan gerak obstacle ke bawah

    void Update()
    {
        // Gerakkan obstacle lurus ke bawah
        transform.Translate(Vector2.down * speed * Time.deltaTime);

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
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null) gm.PlayerTookDamage();

            Destroy(gameObject); // Hancurkan rintangan ini
        }
    }
}
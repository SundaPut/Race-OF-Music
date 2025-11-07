using UnityEngine;

public class LevelChunk : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        // Gerakkan seluruh potongan level ke bawah
        if (gameManager != null)
        {
            transform.Translate(Vector3.down * gameManager.currentGameSpeed * Time.deltaTime);
        }

        // Hapus potongan level ini jika sudah jauh di bawah layar
        // Angka -20f adalah angka aman, bisa disesuaikan dengan tinggi potongan level Anda
        if (transform.position.y < -20f)
        {
            Destroy(gameObject);
        }
    }
}
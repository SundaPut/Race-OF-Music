using UnityEngine;

public class Star : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Hapus star saat disentuh player
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Jika star jatuh keluar layar (misal di bawah Y = -6), hapus otomatis
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }
}

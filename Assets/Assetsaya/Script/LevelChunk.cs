using UnityEngine;

public class LevelChunk : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        // Gerakkan seluruh potongan level ke bawah
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

        // Hapus potongan level ini jika sudah jauh di bawah layar
        // Angka -20f adalah angka aman, bisa disesuaikan dengan tinggi potongan level Anda
        if (transform.position.y < -20f)
        {
            Destroy(gameObject);
        }
    }
}
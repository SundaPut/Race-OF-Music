using UnityEngine;

public class StarCollect : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Cek apakah yang menabrak adalah Player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Star collected!");
            
            // Hapus (hilangkan) objek Star
            Destroy(gameObject);
        }
    }
}

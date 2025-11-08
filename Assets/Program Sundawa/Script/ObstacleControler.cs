using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleControler : MonoBehaviour
{
    // Collider yang bertugas sebagai pemicu (trigger) untuk menambah skor.
    public Collider2D scoreTrigger;

    private bool scoreAdded = false;

    private void Start()
    {
        if (scoreTrigger == null)
        {
            Debug.LogError("Score Trigger tidak terpasang pada " + gameObject.name);
        }
    }

    // Dipanggil saat Collider NON-Trigger bertabrakan
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Panggil GameManager.Instance, bukan ScoreManager.Instance
        if (collision.gameObject.CompareTag("Player") && GameManager.Instance != null)
        {
            // GameManager akan mengurus kerusakan dan Game Over
            GameManager.Instance.PlayerTookDamage();
        }
    }

    // Dipanggil saat Collider Trigger dilewati
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Pastikan yang melewati adalah pemain, trigger yang benar, dan skor belum ditambahkan
        if (other.CompareTag("Player") && other == scoreTrigger && !scoreAdded && GameManager.Instance != null)
        {
            // Tambahkan 1 poin melalui GameManager
            GameManager.Instance.AddScore(1);
            scoreAdded = true;

            // Nonaktifkan trigger agar skor tidak bertambah lagi
            scoreTrigger.enabled = false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Digit Display Configuration")]
    public Sprite[] digitSprites;
    public List<Image> scoreDisplayImages;

    [Header("Digit Positioning")]
    public float digitSpacing = 50f; // NILAI INI PENTING! Sesuaikan di Inspector.
    public float centerPositionX = 0f;

    private int currentScore = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // LOGIKA PANEL START DIHAPUS
        currentScore = 0;
        UpdateScoreDisplay();
    }

    public void AddScore(int pointsToAdd)
    {
        currentScore += pointsToAdd;
        UpdateScoreDisplay();
    }

    // FUNGSI INI DIBUTUHKAN GAMEMANAGER UNTUK HITUNG KECEPATAN
    public int GetCurrentScore()
    {
        return currentScore;
    }

    private void UpdateScoreDisplay()
    {
        DisplayScoreAsImages(currentScore, scoreDisplayImages);
    }

    // FUNGSI BARU UNTUK DIPANGGIL GAMEMANAGER SAAT GAME OVER
    public void DisplayFinalScore(int finalScore, List<Image> displayImages)
    {
        DisplayScoreAsImages(finalScore, displayImages);
    }

    private void DisplayScoreAsImages(int score, List<Image> displayImages)
    {
        string scoreString = score.ToString();
        int digitCount = scoreString.Length;

        if (digitSprites.Length == 0 || displayImages.Count == 0) return;

        // LOGIKA PENEMPATAN HORIZONTAL (Mencegah tumpukan)
        float totalWidth = digitCount * digitSpacing;
        float startX = centerPositionX - (totalWidth / 2f) + (digitSpacing / 2f);

        for (int i = 0; i < displayImages.Count; i++)
        {
            Image digitImage = displayImages[i];
            RectTransform digitRect = digitImage.GetComponent<RectTransform>();

            if (i < digitCount)
            {
                char digitChar = scoreString[i];
                int digitValue = int.Parse(digitChar.ToString());

                digitImage.sprite = digitSprites[digitValue];
                digitImage.enabled = true;

                float currentX = startX + (i * digitSpacing);
                digitRect.anchoredPosition = new Vector2(currentX, digitRect.anchoredPosition.y);
            }
            else
            {
                digitImage.enabled = false;
            }
        }
    }
}
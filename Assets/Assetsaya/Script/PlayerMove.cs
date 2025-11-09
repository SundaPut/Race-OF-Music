using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Tooltip("Posisi X untuk setiap lajur (misal: -4, -2, 0, 2, 4)")]
    public float[] laneXPositions = new float[] { -4f, -2f, 0f, 2f, 4f };

    [Tooltip("Lajur awal pemain (0 = paling kiri, dst.)")]
    public int startingLane = 2; // Lajur tengah untuk 5 lajur adalah indeks 2

    [Tooltip("Durasi untuk menyelesaikan satu langkah")]
    public float moveDuration = 0.1f;

    [Header("Mobile Swipe Settings")]
    [Tooltip("Jarak minimal sentuhan harus bergeser untuk dianggap sebagai swipe.")]
    public float swipeThreshold = 50f;

    private bool isMoving = false;
    private int currentLaneIndex;

    // Variabel untuk melacak sentuhan
    private Vector2 touchStartPos;
    private Vector2 touchEndPos;

    void Start()
    {
        // Atur lajur awal pemain
        currentLaneIndex = Mathf.Clamp(startingLane, 0, laneXPositions.Length - 1);
        transform.position = new Vector3(laneXPositions[currentLaneIndex], transform.position.y, transform.position.z);
    }

    void Update()
    {
        // Jangan proses input baru jika sedang bergerak
        if (isMoving)
        {
            return;
        }

        // --- Deteksi Swipe Mobile ---
        HandleMobileInput();

        // --- Input Keyboard (Opsional: untuk testing di editor) ---
#if UNITY_EDITOR
        HandleKeyboardInput();
#endif
    }

    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveToLane(currentLaneIndex + 1);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveToLane(currentLaneIndex - 1);
        }
    }

    private void HandleMobileInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Simpan posisi awal sentuhan
                touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                touchEndPos = touch.position;
                Vector2 swipeDelta = touchEndPos - touchStartPos;

                // Pastikan gerakan yang dilakukan adalah gerakan horizontal yang signifikan
                if (Mathf.Abs(swipeDelta.x) > swipeThreshold && Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                {
                    // Swipe ke Kanan
                    if (swipeDelta.x > 0)
                    {
                        MoveToLane(currentLaneIndex + 1);
                    }
                    // Swipe ke Kiri
                    else
                    {
                        MoveToLane(currentLaneIndex - 1);
                    }
                }
            }
        }
    }

    private void MoveToLane(int targetLaneIndex)
    {
        // Cek apakah lajur target valid (berada dalam rentang array)
        if (targetLaneIndex >= 0 && targetLaneIndex < laneXPositions.Length)
        {
            currentLaneIndex = targetLaneIndex;
            Vector3 targetPosition = new Vector3(laneXPositions[currentLaneIndex], transform.position.y, transform.position.z);
            StartCoroutine(MoveToPosition(targetPosition));
        }
    }

    private IEnumerator MoveToPosition(Vector3 target)
    {
        isMoving = true;
        float elapsedTime = 0;
        Vector3 startingPos = transform.position;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startingPos, target, (elapsedTime / moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = target; // Pastikan posisi akhir akurat
        isMoving = false;
    }
}
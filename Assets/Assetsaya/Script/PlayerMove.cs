using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Tooltip("Posisi X untuk setiap lajur (misal: -2, 0, 2)")]
    public float[] laneXPositions = new float[] { -4f, -2f, 0f, 2f, 4f };
    
    [Tooltip("Lajur awal pemain (0 = paling kiri, 1 = tengah, dst.)")]
    public int startingLane = 2; // Lajur tengah untuk 5 lajur adalah indeks 2

    [Tooltip("Durasi untuk menyelesaikan satu langkah")]
    public float moveDuration = 0.1f;

    private bool isMoving = false;
    private int currentLaneIndex;

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

        // Deteksi input sekali tekan untuk bergerak ke kanan
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveToLane(currentLaneIndex + 1);
        }
        // Deteksi input sekali tekan untuk bergerak ke kiri
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveToLane(currentLaneIndex - 1);
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

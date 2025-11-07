using UnityEngine;

// Skrip ini membuat latar belakang bergerak ke bawah secara terus-menerus.
public class ScrollingBackground : MonoBehaviour
{
    [Header("Scrolling")]
    public float scrollSpeed = 2f;
    private float backgroundHeight;
    private Vector3 startPosition;

    [Header("Obstacle Spawning")]
    public GameObject obstaclePrefab; // Seret prefab Obstacle ke sini
    public float spawnInterval = 3f;  // Interval kemunculan obstacle dalam detik
    public float minMoveDistance = 2f; // Jarak minimal pergerakan obstacle baru
    public float maxMoveDistance = 4f; // Jarak maksimal pergerakan obstacle baru dari yang lama
    private float lastObstacleX = 0f;  // Menyimpan posisi X obstacle terakhir
    private float nextSpawnTime;
    void Start()
    {
        // Simpan posisi awal dan tinggi dari sprite latar belakang
        startPosition = transform.position;
        backgroundHeight = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        // Hitung posisi baru menggunakan Mathf.Repeat untuk membuat efek looping yang mulus.
        // Time.time * scrollSpeed adalah total jarak yang telah ditempuh.
        // Mathf.Repeat akan membuat nilai ini selalu berada di antara 0 dan backgroundHeight.
        float newY = Mathf.Repeat(Time.time * scrollSpeed, backgroundHeight);
        transform.position = startPosition + Vector3.down * newY;

        // Cek apakah sudah waktunya untuk memunculkan obstacle baru
        if (obstaclePrefab != null && Time.time > nextSpawnTime)
        {
            SpawnObstacle();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnObstacle()
    {
        // Tentukan batas layar
        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float obstacleWidth = obstaclePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float spawnLimit = screenWidth - (obstacleWidth / 2);

        // Tentukan rentang spawn di sisi kiri dan kanan dari obstacle terakhir
        // Rentang Kiri
        float leftRangeMin = Mathf.Max(-spawnLimit, lastObstacleX - maxMoveDistance);
        float leftRangeMax = Mathf.Min(spawnLimit, lastObstacleX - minMoveDistance);

        // Rentang Kanan
        float rightRangeMin = Mathf.Max(-spawnLimit, lastObstacleX + minMoveDistance);
        float rightRangeMax = Mathf.Min(spawnLimit, lastObstacleX + maxMoveDistance);

        // Cek apakah ada ruang yang valid di kedua sisi
        bool canSpawnLeft = leftRangeMin < leftRangeMax;
        bool canSpawnRight = rightRangeMin < rightRangeMax;

        float newX;

        if (canSpawnLeft && canSpawnRight)
        {
            // Jika bisa spawn di kedua sisi, pilih salah satu secara acak
            if (Random.value > 0.5f)
            {
                newX = Random.Range(rightRangeMin, rightRangeMax);
            }
            else
            {
                newX = Random.Range(leftRangeMin, leftRangeMax);
            }
        }
        else if (canSpawnRight)
        {
            // Hanya bisa spawn di kanan
            newX = Random.Range(rightRangeMin, rightRangeMax);
        }
        else
        {
            // Hanya bisa spawn di kiri (atau jika tidak ada ruang sama sekali, akan tetap di sini)
            newX = Random.Range(leftRangeMin, leftRangeMax);
        }

        // Posisi Y untuk spawn (di atas layar)
        float spawnY = 8f;

        // Buat (Instantiate) satu obstacle dan simpan posisinya untuk spawn berikutnya
        Instantiate(obstaclePrefab, new Vector3(newX, spawnY, 0), Quaternion.identity);
        lastObstacleX = newX;

    }
}
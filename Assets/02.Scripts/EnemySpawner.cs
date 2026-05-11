using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public GameObject[] EnemySpawnPointer;
    public float min = 5.0f;
    public float max = 10.0f;
    public int maxSpawn = 3;

    float timer;
    float spawnTime;
    void Start()
    {
        timer = 0f;
        spawnTime = Random.Range(min, max);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnTime)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length < maxSpawn)
            {
                // 플레이어가 존재하는지 확인
                GameObject[] playeres = GameObject.FindGameObjectsWithTag("Player");
                // 플레이어가 존재하면 적을 스폰
                if (playeres.Length > 0)
                {
                    SpawnEnemy();
                }   
            }
            timer = 0f;
            spawnTime = Random.Range(min, max);
        }
    }

    void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, EnemySpawnPointer.Length);
        Instantiate(EnemyPrefab, EnemySpawnPointer[spawnIndex].transform.position, Quaternion.identity);
    }
}

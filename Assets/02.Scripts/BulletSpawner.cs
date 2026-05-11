using UnityEngine;
using UnityEngine.UI;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float min = 0.5f;
    public float max = 3.0f;

    public float SpawnerHP = 5.0f;

    public Transform[] BulletSpawnPointer;


    Transform target;
    float rate; // 총알이 생성되는 간격을 저장하는 변수 (랜덤으로 설정됨)
    float timeAfterSpawn;   // 총알이 생성된 후 경과한 시간을 저장하는 변수 (간격시간)

    void Start()
    {
        timeAfterSpawn = 0f;    // 몇 초뒤에 불러오는지 계산하기 위한 변수 초기화
        rate = Random.Range(min, max); // (0, 3) 0 ~ 2까지 랜덤 숫자 생성
                                       // (0.0f, 3.0f) 0.0 ~ 2.9999999까지 랜덤 숫자 생성
        
        // FindFirstObjectByType<T>()는 씬에서 T 타입의 첫 번째 오브젝트를 찾는 메서드
        // 여기서는 PlayerController 타입의 첫 번째 오브젝트를 찾아서 그 트랜스폼을 target 변수에 할당
        //target = FindFirstObjectByType<PlayerController>().transform;   
    }
    private void Update()
    {
        // 1. 타겟이 존재하는지 확인
        if (target == null)
        {
            SpawnTarget();
            return; // 타겟을 찾을 때까지 이번 프레임은 발사하지 않고 건너뜀
        }

        // 2. 타이머 계산
        timeAfterSpawn += Time.deltaTime;

        // 타이머가 설정된 간격(rate)보다 크거나 같으면 총알을 생성
        if (timeAfterSpawn >= rate)
        {
            timeAfterSpawn = 0f;

            for (int i = 0; i < BulletSpawnPointer.Length; i++)
             {
                 GameObject o = Instantiate(bulletPrefab, BulletSpawnPointer[i].position, BulletSpawnPointer[i].rotation);
                 if (target != null)
                 {
                     o.transform.LookAt(target);
                     this.transform.LookAt(target);
                 }
             }

            rate = Random.Range(min, max);
        }
    }

    public void SpawnTarget()
    {

        // 씬에서 새로 생성된 PlayerController를 찾음
        PlayerController p = FindFirstObjectByType<PlayerController>();

        // PlayerController가 존재한다면 그 트랜스폼을 target으로 설정
        if (p != null)
        {
            target = p.transform;
        }
    }

    public void TakeDamage(float damage)
    {
        SpawnerHP -= damage;
        if (SpawnerHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}

using UnityEngine;
using System.Collections;

public class SpawnerWithWarning : MonoBehaviour
{
    public GameObject targetPrefab;  // 실제로 떨어질 물체
    public GameObject warningPrefab; // 빨간색 경고 프리팹
    public float warningDuration = 1.5f; // 경고가 지속될 시간
    public float spawnInterval = 2.0f;   // 생성 주기
    public float rangeX = 10f;
    public float rangeZ = 10f;
    public float spawnHeight = 10f;
    public float groundY = 0f; // 경고가 표시될 바닥 높이

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // 1. 랜덤한 X, Z 위치 선정 (3D 터레인 기준)
            float randomX = Random.Range(-rangeX, rangeX);
            float randomZ = Random.Range(-rangeZ, rangeZ);

            // 하늘 위 시작점
            Vector3 rayStartPos = new Vector3(transform.position.x + randomX, spawnHeight, transform.position.z + randomZ);

            // 2. 아래로 레이캐스트 발사
            RaycastHit hit;
            // 지면(Terrain)만 감지하도록 레이어를 설정하는 것이 좋습니다.
            if (Physics.Raycast(rayStartPos, Vector3.down, out hit, spawnHeight + 10f))
            {
                Vector3 groundPos = hit.point; // 레이저가 터레인에 닿은 정확한 지점

                // 3. 경고 표시 소환 (지면보다 아주 살짝 위에 띄워야 깜빡거림이 없습니다)
                GameObject warning = Instantiate(warningPrefab, groundPos + Vector3.up * 0.05f, Quaternion.identity);

                // 💡 팁: 경고 표시가 지형의 기울기에 맞춰 눕게 하려면?
                warning.transform.up = hit.normal;

                yield return new WaitForSeconds(warningDuration);

                // 4. 물체 소환 및 경고 삭제
                Destroy(warning);
                Instantiate(targetPrefab, rayStartPos, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
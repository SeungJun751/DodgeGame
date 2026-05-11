using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float health = 3f; // 적의 체력
    private Rigidbody rb;
    public Transform Trplayer;
    public float PlayerDetectionRange = 10f; // 플레이어를 감지하는 범위
    public float AttackRange = 5f; // 공격 범위
    public GameObject CoinPrefab;

    private NavMeshAgent agent;
    private Animator ani;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
    }

    public void Update()
    {
        if (Trplayer == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Trplayer = player.transform;
                agent.SetDestination(Trplayer.position);
            }
        }
        if (Trplayer != null)
        {
            agent.SetDestination(Trplayer.position);

            float distanceToPlayer = Vector3.Distance(transform.position, Trplayer.position);
            if (Vector3.Distance(transform.position, Trplayer.position) <= PlayerDetectionRange)
            {
                ani.SetBool("isAttack", true);
            }
            else
            {
                ani.SetBool("isAttack", false);
            }
        }


        if (health <= 0)
        {
            Destroy(gameObject); // 체력이 0 이하가 되면 적 오브젝트 제거
        }
    }

    public void Attack()
    {
        Debug.Log("Enemy attacks!");
        // 공격 범위 내에 있는 플레이어를 감지하여 공격
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, AttackRange);
        // 공격 범위 내에 있는 모든 콜라이더를 검사
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                PlayerController player = hitCollider.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.Die(); // 플레이어의 Die() 메서드 호출
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
            Destroy(gameObject);
        }
    }

    public void Die ()
    {
        Instantiate(CoinPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

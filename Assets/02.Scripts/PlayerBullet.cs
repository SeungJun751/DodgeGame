using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 10f; // 총알의 이동 속도
    public float damage = 1f; // 총알이 적에게 입히는 피해량
    private Rigidbody rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        rb.linearVelocity = transform.forward * speed;

    }


    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy Hit!");
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject); 
        }
    }
}

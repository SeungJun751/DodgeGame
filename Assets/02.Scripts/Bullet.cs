using UnityEngine;
using UnityEngine.Rendering;

public class Bullet : MonoBehaviour
{
    public float speed = 15.0f;
    private Rigidbody rb;

    //public GameObject FX;
    public AudioSource BulletAudio;
    public AudioClip BulletClip;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        BulletAudio = GetComponent<AudioSource>();
        rb.linearVelocity = transform.forward * speed;  // 총알이 생성될 때, 총알의 forward 방향으로 speed 속도로 이동하도록 설정
        Destroy(gameObject, 5.0f);
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {    
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerController.HasHit) return;
            playerController.HasHit = true;
            BulletAudio.PlayOneShot(BulletClip);
            // identity는 회전을 하지 않겠다는 의미입니다.
            // 인스턴스화할 때, FX의 위치는 other.transform.position으로 설정하고, 회전은 하지 않도록 설정합니다.
            //Instantiate(FX, other.transform.position, Quaternion.identity);

            other.gameObject.GetComponent<PlayerController>().Die();
            Destroy(gameObject);
            Debug.Log("Bullet hit the player!");
        }
        if (other.gameObject.CompareTag("WALL"))
        {
            Destroy(gameObject);
        }
    }
}

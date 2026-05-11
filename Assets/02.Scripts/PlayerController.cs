using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 10.0f;
    //public float jumpForce = 5.0f;
    public GameObject player;

    public float PlayerRotationSpeed = 100.0f;
    //private float xRotation = 0f;

    public GameObject BulletSpawner;
    public GameObject PlayerBullet;
    private static bool isPlayerShot = false;

    public GameObject FX;
    public AudioSource DieSound;
    public AudioClip DieClip;

    private Animator ani;

    public bool HasHit = false;
    private VariableJoystick joy;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        joy = GameObject.FindAnyObjectByType<VariableJoystick>();
        DieSound = GetComponent<AudioSource>();

        isPlayerShot = false;
    }

    void Update()
    {
        //float x = Input.GetAxis("Horizontal");
        //float z = Input.GetAxis("Vertical");

        //Vector3 moveDirection = (transform.right * x + transform.forward * z).normalized;
        //Vector3 movement = moveDirection * speed;

        ////Vector3 movement = new Vector3(joy.Horizontal, 0, joy.Vertical);

        //rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);

        //// magnitude가 0보다 크면 이동 중이므로 Move 애니메이션을 재생, 그렇지 않으면 Idle 애니메이션을 재생
        //ani.SetBool("Move", moveDirection.magnitude > 0);

        float x = joy.Horizontal;
        float z = joy.Vertical;

        Vector3 moveDirection = new Vector3(x, 0, z).normalized;

        Vector3 movement = moveDirection * speed;
        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);


        if (moveDirection.magnitude > 0.1f) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        ani.SetBool("Move", moveDirection.magnitude > 0.1f);

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 spawnPos = transform.position + Vector3.up * 1.5f;
        //    GameObject bullet = Instantiate(PlayerBullet, BulletSpawner.transform.position, Quaternion.identity);
        //    bullet.transform.LookAt(spawnPos + transform.forward * 10f);
        //    Destroy(bullet, 3.0f);
        //}
        if (isPlayerShot == true)
        {
            ExcuteShot();
            isPlayerShot = false;
        }
    }

    public void Shot()
    {
        isPlayerShot = true;
    }

    public void ExcuteShot()
    {
        // 가장 가까운 적 찾기
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        GameObject bullet = Instantiate(PlayerBullet, BulletSpawner.transform.position, Quaternion.identity);

        // 적이 있으면 그 방향으로, 없으면 앞으로 발사
        if (closestEnemy != null)
        {
            Vector3 direction = (closestEnemy.transform.position - BulletSpawner.transform.position).normalized;
            bullet.transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            bullet.transform.rotation = BulletSpawner.transform.rotation;
        }

        Destroy(bullet, 3.0f);
    }
    public void Die()
    {
        HasHit = true;
        GameManager GameManager = FindFirstObjectByType<GameManager>();
        Enemy enemy = FindFirstObjectByType<Enemy>();
        GameManager.Count();
        GameManager.SpawnPlayer();
        GameManager.EnemyReset();
        if (DieClip != null)
        {
            AudioSource.PlayClipAtPoint(DieClip, transform.position);
        }
        Destroy(gameObject);
        FX = Instantiate(FX, transform.position, Quaternion.identity);
        
    }
}

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;   
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public GameObject Player;

    public GameObject SpawnPointer;
    public Image[] hearts;
    private int count = 5;
    public Text timer;
    private float timerCount = 0f;
    public int coinCount = 0;
    public Text CoinText;
    public Image CoinImage;
    public Text BestCoinText;
    public GameObject GameOver;
    public Text recordText;
    public GameObject JoyStick;

    public Transform[] EnemyRandomSpawner;
    

    private bool isGameOver = false;   
    

    private void Awake()
    {
        Time.timeScale = 1.0f;
        count = 5;
        timerCount = 0;
        timer.text = "Time: " + timerCount.ToString("N0") + "s";
        coinCount = 0;
        CoinText.text = " X " + coinCount.ToString();
    }
    void Start()
    {
        GameOver.SetActive(false);
        JoyStick.SetActive(true);
        timer.enabled = true;
        CoinText.enabled = true;
        CoinImage.enabled = true;
        GameObject obj = Instantiate(Player, SpawnPointer.transform.position, Quaternion.identity);
        this.transform.position = SpawnPointer.transform.position;
        BulletSpawner bulletSpawner = FindFirstObjectByType<BulletSpawner>();
        // SpawnTarget() 메서드를 호출하여 총알 스포너가 새로 생성된 플레이어를 타겟으로 설정하도록 함
        if (bulletSpawner != null)
        {
            bulletSpawner.SpawnTarget();
        }
        
    }   void Update()
    {
        if (!isGameOver)
        {
            timerCount += Time.deltaTime;
            timer.text = "Time: " + timerCount.ToString("N0");
        }
    }
    public void SpawnPlayer()
    {
        BulletSpawner bulletSpawner = FindFirstObjectByType<BulletSpawner>();
        bulletSpawner.SpawnTarget();
        StartCoroutine(Respawn());
        Bullet bullet = FindFirstObjectByType<Bullet>();
        if (count == 0)
        {
            JoyStick.SetActive(false);
            isGameOver = true;
            Debug.Log("Game Over!");
            Time.timeScale = 0.0f;
            GameOver.SetActive(true);
            timer.enabled = false;
            CoinText.enabled = false;
            CoinImage.enabled = false;
            // 게임 오버 시점에 최고 기록을 저장하는 로직
            float bestTime = PlayerPrefs.GetFloat("BestTime");
            // 현재 기록이 이전 최고 기록보다 크면 최고 기록을 업데이트
            if (timerCount > bestTime)
            {
                // 최고 기록을 업데이트하고 PlayerPrefs에 저장
                bestTime = timerCount;
                PlayerPrefs.SetFloat("BestTime", bestTime);
            }
            float bestCoin = PlayerPrefs.GetFloat("BestCoin");
            if (coinCount > bestCoin)
            {
                bestCoin = coinCount;
                PlayerPrefs.SetFloat("BestCoin", bestCoin);
            }
            recordText.text = "Best Time: " + bestTime.ToString("N0") + "s";
            BestCoinText.text = "Best Coin: " + bestCoin.ToString();
        }
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3.0f);
        Debug.Log("Respawned!");
        GameObject obj = Instantiate(Player, SpawnPointer.transform.position, Quaternion.identity);
        this.transform.position = SpawnPointer.transform.position;
        BulletSpawner bulletSpawner = FindFirstObjectByType<BulletSpawner>();
        // SpawnTarget() 메서드를 호출하여 총알 스포너가 새로 생성된 플레이어를 타겟으로 설정하도록 함
        if (bulletSpawner != null)
        {
            bulletSpawner.SpawnTarget();
        }
        yield break;
    }
    public void Count ()
    {
        count--;
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < count)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void CoinCount()
    {
        coinCount++;
        CoinText.text = " X " + coinCount.ToString();
    }
    public void EnemyReset()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}

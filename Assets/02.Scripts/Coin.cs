using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject coin;
    public float rotationSpeed = 50f;

    public void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.CoinCount();
            Destroy(coin);
        }
    }
}

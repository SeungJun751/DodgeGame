using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1.0f;
    }
    public void GameStart ()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void GameExit()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Intro");
    }

}

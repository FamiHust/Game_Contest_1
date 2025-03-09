using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject OverPanel;
    public bool isGameOver = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        OverPanel.SetActive(true);

        if (PlayerHealth.Instance != null)
        {
            GameObject playerObj = PlayerHealth.Instance.gameObject;
            PlayerHealth.Instance = null;
            Destroy(playerObj);
        }
    }

    
    public void QuitGame()
    {
        Application.Quit();
    }
}
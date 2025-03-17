using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject OverPanel;
    public GameObject WinPanel;

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

    public void GameWin()
    {
        StartCoroutine(GameWinCoroutine());
    }

    private IEnumerator GameWinCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        WinPanel.SetActive(true);

        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance = null;
        }
    }

    public void LoadingScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
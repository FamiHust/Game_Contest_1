using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject OverPanel;
    public GameObject WinPanel;
    public GameObject PausePanel;

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
        SoundManager.PlaySound(SoundType.GameOver);

        MoneyManager.Instance.SpendMoney(100);

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
        SoundManager.PlaySound(SoundType.Victory);

        MoneyManager.Instance.AddMoney(100);

        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance = null;
        }
    }

    public void LoadingScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }

    public void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
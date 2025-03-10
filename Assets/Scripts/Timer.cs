using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class Timer : MonoBehaviour
{
    [SerializeField] private float remainingTime;
    public TextMeshProUGUI timerText;

    private bool isBlinking = false; 
    private float blinkInterval = 0.5f; 
    private float blinkTimer = 0f;
    private float scaleSpeed = 0.2f; 
    private float maxScale = 1.1f;
    private float minScale = 1.0f;

    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime < 0)
        {
            remainingTime = 0;
            timerText.color = Color.red;
            PlayerHealth.Instance.Die();
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);

        BlinkText();
    }

    private void BlinkText()
    {
        if (remainingTime <= 11f)
        {
            isBlinking = true; 
            ScaleText(); 
        }

        if (isBlinking)
        {
            blinkTimer += Time.deltaTime;
            if (blinkTimer >= blinkInterval)
            {
                timerText.color = (timerText.color == Color.red) ? Color.white : Color.red;
                blinkTimer = 0f; 
            }
        }
    }

    private void ScaleText()
    {
        float scale = Mathf.PingPong(Time.time * scaleSpeed, maxScale - minScale) + minScale;
        timerText.transform.localScale = new Vector3(scale, scale, 1);
    }
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    public Image fillBar;
    public TextMeshProUGUI valueTest;
    private Coroutine currentCoroutine;

    public void UpdateBar(int currentValue, int maxValue)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(SmoothUpdateBar(currentValue, maxValue));
    }

    private IEnumerator SmoothUpdateBar(int targetValue, int maxValue)
    {
        float startFill = fillBar.fillAmount;
        float endFill = (float)targetValue / maxValue;
        float elapsed = 0f;
        float duration = 0.3f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fillBar.fillAmount = Mathf.Lerp(startFill, endFill, elapsed / duration);
            valueTest.text = Mathf.RoundToInt(Mathf.Lerp(startFill * maxValue, endFill * maxValue, elapsed / duration)).ToString()
                + "/" + maxValue.ToString();
            yield return null;
        }

        fillBar.fillAmount = endFill;
        valueTest.text = targetValue.ToString() + "/" + maxValue.ToString();
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class Timer : MonoBehaviour
{
    [SerializeField] private float remainingTime;
    public TextMeshProUGUI timerText;

    // private bool isBlinking = false; // Biến kiểm tra trạng thái nhấp nháy
    // private float blinkInterval = 0.5f; // Thời gian giữa các lần nhấp nháy
    // private float blinkTimer = 0f; // Bộ đếm thời gian cho nhấp nháy
    // private float scaleSpeed = 0.2f; // Tốc độ phóng to/thu nhỏ
    // private float maxScale = 1.1f; // Kích thước tối đa
    // private float minScale = 1.0f; // Kích thước tối thiểu

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

        // // Kiểm tra xem thời gian còn lại có dưới 10 giây không
        // if (remainingTime <= 11f)
        // {
        //     isBlinking = true; // Bắt đầu nhấp nháy
        //     ScaleText(); // Gọi phương thức để phóng to/thu nhỏ văn bản
        // }

        // // Nhấp nháy văn bản
        // if (isBlinking)
        // {
        //     blinkTimer += Time.deltaTime;
        //     if (blinkTimer >= blinkInterval)
        //     {
        //         // Đổi màu văn bản giữa đỏ và trong suốt
        //         timerText.color = (timerText.color == Color.red) ? Color.white : Color.red;
        //         blinkTimer = 0f; // Đặt lại bộ đếm thời gian
        //     }
        // }
    }

    // private void ScaleText()
    // {
    //     // Sử dụng Mathf.PingPong để tạo hiệu ứng phóng to/thu nhỏ mượt mà
    //     float scale = Mathf.PingPong(Time.time * scaleSpeed, maxScale - minScale) + minScale;
    //     timerText.transform.localScale = new Vector3(scale, scale, 1);
    // }
}
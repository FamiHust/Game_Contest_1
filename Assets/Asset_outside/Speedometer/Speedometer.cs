using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Speedometer : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxNeedleRotation = -220f; 
    [SerializeField] private float minNeedleRotation = 40f; 
    [SerializeField] private float needleSmoothTime = 0.1f;
    private float currentSpeed = 0f;
    private float speedVelocity = 0f;

    public TextMeshProUGUI speedText; 
    public Rigidbody2D playerRb;
    public RectTransform needle; 

    private void Update()
    {
        if (playerRb != null)
        {
            float targetSpeed = playerRb.velocity.magnitude; 
            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, needleSmoothTime);
            
            speedText.text = (currentSpeed*10).ToString("F2"); 
            
            float needleRotation = Mathf.Lerp(minNeedleRotation, maxNeedleRotation, currentSpeed / maxSpeed);
            needle.rotation = Quaternion.Euler(0, 0, needleRotation);
        }
    }
}

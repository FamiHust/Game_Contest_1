using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    private const float MAX_SPEED_ANGLE = -20;
    private const float ZERO_SPEED_ANGLE = 230;

    private Transform needleTransform;
    private Transform speedLabelTemplateTransform;

    private float speedMax;
    private float speed;

    private void Awake()
    {
        needleTransform = transform.Find("needle");
        speedLabelTemplateTransform = transform.Find("speedLabelTemplate");
        speedLabelTemplateTransform.gameObject.SetActive(false);

        speedMax = PlayerController.Instance != null ? PlayerController.Instance.moveSpeed : 5f;

        CreateSpeedLabels();
    }

    private void Update()
    {
        if (PlayerController.Instance != null)
        {
            speed = PlayerController.Instance.rb.velocity.magnitude;
        }

        needleTransform.eulerAngles = new Vector3(0, 0, GetSpeedRotation());
    }

    private void CreateSpeedLabels()
    {
        int labelAmount = 10;
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;

        for (int i = 0; i <= labelAmount; i++)
        {
            Transform speedLabelTransform = Instantiate(speedLabelTemplateTransform, transform);
            float labelSpeedNormalized = (float)i / labelAmount;
            float speedLabelAngle = ZERO_SPEED_ANGLE - labelSpeedNormalized * totalAngleSize;
            speedLabelTransform.eulerAngles = new Vector3(0, 0, speedLabelAngle);
            speedLabelTransform.Find("speedText").GetComponent<Text>().text = Mathf.RoundToInt(labelSpeedNormalized * speedMax).ToString();
            speedLabelTransform.Find("speedText").eulerAngles = Vector3.zero;
            speedLabelTransform.gameObject.SetActive(true);
        }

        needleTransform.SetAsLastSibling();
    }

    private float GetSpeedRotation()
    {
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;
        float speedNormalized = speed / speedMax;
        return ZERO_SPEED_ANGLE - speedNormalized * totalAngleSize;
    }
}

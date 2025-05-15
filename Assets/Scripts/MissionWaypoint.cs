using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionWaypoint : MonoBehaviour
{
    public Image img;
    public Transform target;
    public TextMeshProUGUI meter;
    public Vector3 offset;

    public RectTransform boundaryArea;

    // private void Update()
    // {
    //     Vector2 pos = Camera.main.WorldToScreenPoint(target.position + offset);

    //     // Kiểm tra xem target có nằm trước hay sau camera
    //     if (Vector3.Dot((target.position - transform.position), transform.forward) < 0)
    //     {
    //         if (pos.x < Screen.width / 2)
    //         {
    //             pos.x = boundaryArea.rect.width;
    //         }
    //         else
    //         {
    //             pos.x = 0;
    //         }
    //     }

    //     // Lấy kích thước vùng giới hạn
    //     Vector3[] corners = new Vector3[4];
    //     boundaryArea.GetWorldCorners(corners);

    //     float minX = corners[0].x;
    //     float maxX = corners[2].x;
    //     float minY = corners[0].y;
    //     float maxY = corners[2].y;

    //     // Giới hạn icon waypoint trong vùng cố định
    //     pos.x = Mathf.Clamp(pos.x, minX, maxX);
    //     pos.y = Mathf.Clamp(pos.y, minY, maxY);

    //     img.transform.position = pos;
    //     meter.text = ((int)Vector3.Distance(target.position, transform.position)).ToString() + "m";

    //     // Xoay icon chỉ hướng
    //     Vector3 direction = (target.position - transform.position).normalized;
    //     float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
    //     img.transform.rotation = Quaternion.Euler(0, 0, -angle);
    // }
    private void Update()
    {
        Vector2 pos = Camera.main.WorldToScreenPoint(target.position + offset);

        // Kiểm tra xem target có nằm trước hay sau camera
        if (Vector3.Dot((target.position - transform.position), Camera.main.transform.forward) < 0)
        {
            pos = -pos;
        }

        // Lấy kích thước vùng giới hạn
        Vector3[] corners = new Vector3[4];
        boundaryArea.GetWorldCorners(corners);

        float minX = corners[0].x;
        float maxX = corners[2].x;
        float minY = corners[0].y;
        float maxY = corners[2].y;

        // Giới hạn icon waypoint trong vùng cố định
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        img.transform.position = pos;
        meter.text = ((int)Vector3.Distance(target.position, transform.position)).ToString() + "m";

        // Xoay icon chỉ hướng
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        Vector3 direction = (screenPos - screenCenter).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        img.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

}

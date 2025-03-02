using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D customCursor; // Icon chuột mới
    public Vector2 hotspot = Vector2.zero; // Điểm tâm của icon chuột

    void Start()
    {
        if (customCursor != null)
        {
            Cursor.SetCursor(customCursor, hotspot, CursorMode.Auto);
        }
    }
}

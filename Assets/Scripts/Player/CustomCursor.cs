using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D customCursor; 
    public Vector2 hotspot = Vector2.zero;

    void Start()
    {
        if (customCursor != null)
        {
            Cursor.SetCursor(customCursor, hotspot, CursorMode.Auto);
        }
    }
}

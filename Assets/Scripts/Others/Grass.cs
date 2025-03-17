using UnityEngine;

public class Grass : MonoBehaviour
{
    public float fadeAmount = 0.1f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            Color currentColor = spriteRenderer.color;
            float newAlpha = Mathf.Clamp(currentColor.a - fadeAmount, 0f, 1f);
            spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))   
        {
            spriteRenderer.color = originalColor;
        }
    }
}

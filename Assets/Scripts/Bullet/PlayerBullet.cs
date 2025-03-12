using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int damage = 10; 
    private Vector2 hitDirection;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Obstacle"))
        {
            // SoundManager.PlaySound(SoundType.Obstacle);
            gameObject.SetActive(false);
        }
    }
}
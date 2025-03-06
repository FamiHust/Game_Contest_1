using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int damage = 10; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
            // Destroy(gameObject); 
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Obstacle"))
        {
            SoundManager.PlaySound(SoundType.Obstacle);
            // Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}
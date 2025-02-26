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
            Destroy(gameObject); 
        }
        else if (other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}

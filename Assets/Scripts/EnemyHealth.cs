using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public TankType tankType; 
    private int currentHealth;
    private Animator anim;

    private void Awake() 
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (tankType == null)
        {
            return;
        }
        currentHealth = tankType.health;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        anim.SetTrigger("isDie");
        Destroy(gameObject, 1f);
    }
}


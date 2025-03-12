using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private int currentHealth;

    private EnemyController enemyController;
    private Animator anim;
    public GameObject Gun;
    public TankType tankType; 

    private void Awake() 
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
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
        anim.SetTrigger("isHurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (enemyController != null)
        {
            enemyController.Die();
            Gun.SetActive(true);
        }
    }
}

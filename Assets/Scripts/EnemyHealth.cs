using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    private EnemyController enemyController;
    public TankType tankType; 
    private int currentHealth;
    private Animator anim;
    private Color originalColor;
    public GameObject Gun;

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

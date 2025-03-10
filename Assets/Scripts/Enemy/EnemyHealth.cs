using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    private int currentHealth;

    private EnemyController enemyController;
    private Animator anim;
    private Color originalColor;
    public GameObject Gun;
    public TankType tankType; 
    private KnockBack knockBack;

    private void Awake() 
    {
        anim = GetComponent<Animator>();
        knockBack = GetComponent<KnockBack>();
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

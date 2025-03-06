using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float smoothTime = 0.2f;
    private float lastFireTime;
    
    public TankType tankType; 
    public Transform firePoint; 
    
    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 velocity;
    private bool isWalking;
    private bool isAttacking;
    private bool isDead = false;

    private void Start()
    {
        player = PlayerController.Instance?.transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (player == null || PlayerHealth.Instance == null || PlayerHealth.Instance.currentHealth <= 0) 
        {
            StopMovement();
            UnAttack();
            return;
        }

        if (distanceToPlayer <= tankType.attackRange)
        {
            Attack();
            StopMovement();
        }
        else if (distanceToPlayer <= tankType.detectionRange)
        {
            MoveTowardsPlayer();
            UnAttack();
        }
        else
        {
            StopMovement();
            UnAttack();
        }
    }

    public void StopMovement()
    {
        if (!isWalking) return;

        rb.velocity = Vector2.zero;
        isWalking = false;
        anim.SetBool("isWalking", false);
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = Vector2.SmoothDamp(rb.velocity, direction * tankType.moveSpeed, ref velocity, smoothTime);

        RotateTowards(direction);

        if (!isWalking)
        {
            isWalking = true;
            anim.SetBool("isWalking", true);
        }
    }

    private void Attack()
    {
        if (isDead) return;
        
        BulletSpawner();
        isAttacking = true;
        anim.SetBool("isAttacking", true);
    }
    
    public void UnAttack()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", false);
    }

    private void BulletSpawner()
    {
        if (isDead || PlayerHealth.Instance == null || PlayerHealth.Instance.currentHealth <= 0 || Time.time - lastFireTime < 1f / tankType.fireRate) return;

        lastFireTime = Time.time;
        if (tankType.bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(tankType.bulletPrefab, firePoint.position, firePoint.rotation, null);
        EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
        SoundManager.PlaySound(SoundType.FireBullet);
    }

    private void RotateTowards(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = Mathf.LerpAngle(rb.rotation, angle, Time.deltaTime * tankType.rotationSpeed);
    }


    public void Die()
    {
        if (isDead) return;

        isDead = true;
        StopMovement();
        UnAttack();
        anim.SetTrigger("isDie");

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        this.enabled = false;
    }


    private void OnDrawGizmos()
    {
        if (tankType == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, tankType.detectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, tankType.attackRange);
    }
}

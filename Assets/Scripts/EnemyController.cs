// using UnityEngine;

// public class EnemyController : MonoBehaviour
// {
//     [SerializeField] private float smoothTime = 0.2f;
//     private float lastFireTime;
    
//     public TankType tankType; 
//     public Transform firePoint; 
    
//     private Transform player;
//     private Rigidbody2D rb;
//     private Animator anim;
//     private Vector2 velocity;
//     private bool isWalking;
//     private bool isAttacking;

//     private void Start()
//     {
//         if (tankType == null)
//         {
//             enabled = false;
//             return;
//         }

//         player = PlayerController.Instance?.transform;
//         rb = GetComponent<Rigidbody2D>();
//         anim = GetComponent<Animator>();
//     }

//     private void FixedUpdate()
//     {
//         if (player == null) return;

//         float distanceToPlayer = Vector2.Distance(transform.position, player.position);

//         if (distanceToPlayer <= tankType.attackRange)
//         {
//             Attack();
//             StopMovement();
//         }
//         else if (distanceToPlayer <= tankType.detectionRange)
//         {
//             MoveTowardsPlayer();
//         }
//         else
//         {
//             StopMovement();
//         }
//     }

//     private void StopMovement()
//     {
//         if (!isWalking) return;

//         rb.velocity = Vector2.zero;
//         isWalking = false;
//         anim.SetBool("isWalking", false);
//     }

//     private void MoveTowardsPlayer()
//     {
//         Vector2 direction = (player.position - transform.position).normalized;
//         rb.velocity = Vector2.SmoothDamp(rb.velocity, direction * tankType.moveSpeed, ref velocity, smoothTime);
//         RotateTowards(direction);

//         if (!isWalking)
//         {
//             isWalking = true;
//             anim.SetBool("isWalking", true);
//         }
//     }

//     private void Attack()
//     {
//         if (Time.time - lastFireTime < 1f / tankType.fireRate) return;

//         lastFireTime = Time.time;
//         if (tankType.bulletPrefab == null || firePoint == null) return;

//         GameObject bullet = Instantiate(tankType.bulletPrefab, firePoint.position, firePoint.rotation, null);
//         EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
//     }

//     private void RotateTowards(Vector2 direction)
//     {
//         float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
//         transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * tankType.rotationSpeed);
//     }

//     private void OnDrawGizmos()
//     {
//         if (tankType == null) return;

//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(transform.position, tankType.detectionRange);
//         Gizmos.color = Color.yellow;
//         Gizmos.DrawWireSphere(transform.position, tankType.attackRange);
//     }
// }
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

    private void Start()
    {
        if (tankType == null)
        {
            enabled = false;
            return;
        }

        player = PlayerController.Instance?.transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= tankType.attackRange)
        {
            Attack();
            StopMovement();
        }
        else if (distanceToPlayer <= tankType.detectionRange)
        {
            MoveTowardsPlayer();
            isAttacking = false;
            anim.SetBool("isAttacking", false);
        }
        else
        {
            StopMovement();
            isAttacking = false;
            anim.SetBool("isAttacking", false);
        }
    }

    private void StopMovement()
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
        if (Time.time - lastFireTime < 1f / tankType.fireRate) return;

        lastFireTime = Time.time;
        if (tankType.bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(tankType.bulletPrefab, firePoint.position, firePoint.rotation, null);
        EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();

        isAttacking = true;
        anim.SetBool("isAttacking", true);
    }

    private void RotateTowards(Vector2 direction)
    {
        float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * tankType.rotationSpeed);
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

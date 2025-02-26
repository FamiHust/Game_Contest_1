using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float smoothTime = 0.2f;
    private float lastFireTime = 0f;

    public TankType tankType; 
    public Transform firePoint; 
    private Transform player;
    private Rigidbody2D rb;
    private Vector2 velocity = Vector2.zero;
    private Animator anim;

    private bool isWalking = false;

    private void Start()
    {
        if (tankType == null)
        {
            return;
        }

        player = PlayerController.Instance?.transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (player == null || tankType == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= tankType.attackRange)
        {
            Attack();
            EnemyStop();
        }
        else if (distanceToPlayer <= tankType.detectionRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            EnemyStop();
        }
    }



    private void EnemyStop()
    {
        rb.velocity = Vector2.zero;
        if (isWalking)
        {
            isWalking = false;
            anim.SetBool("isWalking", false);
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 targetVelocity = direction * tankType.moveSpeed;
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smoothTime);
        RotateTowardsPlayer(direction);

        if (!isWalking)
        {
            isWalking = true;
            anim.SetBool("isWalking", true);
        }
    }

    private void Attack()
    {
        if (Time.time - lastFireTime >= 1f / tankType.fireRate)
        {
            lastFireTime = Time.time;
            if (tankType.bulletPrefab != null && firePoint != null)
            {
                Instantiate(tankType.bulletPrefab, firePoint.position, firePoint.rotation);
            }
        }
    }

    private void RotateTowardsPlayer(Vector2 direction)
    {
        float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tankType.rotationSpeed);
    }

    private void OnDrawGizmos()
    {
        if (tankType != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, tankType.detectionRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, tankType.attackRange);
        }
    }
}

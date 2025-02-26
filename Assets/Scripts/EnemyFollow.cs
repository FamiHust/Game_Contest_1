using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 3f; 
    public float detectionRange = 5f; 
    public float rotationSpeed = 2f; 
    public float movementDelay = 0.5f; 
    private Transform player;
    private bool isPlayerInRange = false;
    private float movementCooldown = 0f;

    void Start()
    {
        FindPlayer();
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
            if (player == null) return;
        }
        EnemyFollowPlayer();
    }

    private void EnemyFollowPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        isPlayerInRange = distance <= detectionRange;

        if (isPlayerInRange)
        {
            RotateTowardsPlayer();
                
            if (movementCooldown <= 0f)
            {
                MoveTowardsPlayer();
                movementCooldown = movementDelay;
            }
        }
        
        if (movementCooldown > 0f)
        {
            movementCooldown -= Time.deltaTime;
        }
    }

    void MoveTowardsPlayer()
    {
        if (player == null) return;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    void RotateTowardsPlayer()
    {
        if (player == null) return;
        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

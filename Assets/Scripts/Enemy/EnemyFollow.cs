using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private float detectionRange = 5f; 
    [SerializeField] private float rotationSpeed = 2f; 
    private bool isPlayerInRange = false;
    private Transform player;

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
        }
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

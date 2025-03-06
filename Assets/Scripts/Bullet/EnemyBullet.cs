using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 3f;
    private Vector2 direction;
    private Animator anim;

    private void Awake() 
    {
        anim = GetComponent<Animator>();    
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            direction = (player.transform.position - transform.position).normalized;
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            // SoundManager.PlaySound(SoundType.Gunshot);
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Obstacle"))
        {
            SoundManager.PlaySound(SoundType.Obstacle);
            gameObject.SetActive(false);
        }
    }
}
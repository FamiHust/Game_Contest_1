using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float smoothTime = 0.1f; 

    private Animator anim;
    private Rigidbody2D rb;
    [HideInInspector] public Vector2 moveInput;
    private Vector2 velocity = Vector2.zero;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveInput = new Vector2(horizontal, vertical).normalized;

        if (moveInput != Vector2.zero)
        {
            anim.SetBool("isWalking", true);
            FaceMovementDirection();
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        SpeedModify();
    }

    private void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
    }

    private void SpeedModify()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            moveSpeed += 0.1f;
            if(moveSpeed >= 6f)
            {
                moveSpeed = 6f;
            }
        }
        else
        {
            moveSpeed = 3f;
        }
    }

    private void FaceMovementDirection()
    {
        if (moveInput != Vector2.zero)
        {
            float angle = Mathf.Atan2(-moveInput.x, moveInput.y) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}

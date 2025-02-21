using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private Animator anim;

    public float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 velocity = Vector2.zero;
    [SerializeField] private float smoothTime = 0.1f; // Điều chỉnh thời gian làm mượt

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
    }

    // private void FixedUpdate()
    // {
    //     if (moveInput != Vector2.zero)
    //     {
    //         Vector2 targetVelocity = moveInput * moveSpeed;
    //         rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smoothTime);
    //     }
    //     else
    //     {
    //         rb.velocity = Vector2.zero;
    //     }
    // }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
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

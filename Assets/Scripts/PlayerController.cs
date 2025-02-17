using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private Animator anim;

    public float moveSpeed;
    [SerializeField] private float rotationSpeed = 5f;
    private bool isWalking = false;
    private bool isAttacking = false;

    private Rigidbody2D rb;
    private Vector2 moveInput;

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
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput != Vector2.zero)
        {
            moveInput = moveInput.normalized; // Giữ nguyên hướng nhưng đảm bảo tốc độ không tăng lên

            FaceMovementDirection();
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        Shooting();
    }

    private void FixedUpdate()
    {
        Vector2 targetPosition = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(Vector2.Lerp(rb.position, targetPosition, 0.1f)); 
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

    private void Shooting()
    {
        
    }
}
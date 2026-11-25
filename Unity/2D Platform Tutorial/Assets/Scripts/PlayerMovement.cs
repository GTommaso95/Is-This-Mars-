using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 15f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public AudioClip jumpSound;

    private Rigidbody2D rb;
    private Animator animator;
    private float moveInput;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        GetInput();
        FlipSprite();
        HandleJump();
        CheckGround();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void GetInput()
    {
        moveInput = 0f;

        if (Keyboard.current.leftArrowKey.isPressed)
        {
            moveInput = -1f;
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            moveInput = 1f;
        }
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocityY);
    }

    void FlipSprite()
    {
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(-7, 7, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(7, 7, 1);
        }
    }

    void HandleJump()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            Jump(jumpForce);
        }
    }
    public void Jump(float force)
    {
        rb.linearVelocityY = force;
        AudioManager.Instance.PlaySound(jumpSound);
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (!isGrounded)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius);
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Box"))
                {
                    isGrounded = true;
                    break;
                }
            }
        }
    }

    void UpdateAnimation()
    {
        if (!isGrounded)
        {
            if (rb.linearVelocityY > 0)
            {
                animator.Play("Player_Jump");
            }
            else
                animator.Play("Player_Fall");
        }
        else if(moveInput != 0f)
        {
            animator.Play("Player_Run");
        }
        else
            animator.Play("Player_Idle");
    }
}

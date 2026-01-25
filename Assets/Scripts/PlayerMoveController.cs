using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float climbSpeed = 5f;
    public float jumpForce = 14f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    private Animator animator;

    private float moveInput;
    private float verticalInput;
    private float defaultGravityScale;
    private bool isClimbing;

    public GameObject bulletPrefab;
    public Transform gunTip;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        defaultGravityScale = rb.gravityScale;
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        bool isGrounded = capsuleCollider.IsTouchingLayers(groundLayer);

        // Update basic animations
        animator.SetBool("isMoving", moveInput != 0);
        animator.SetBool("isJumping", !isGrounded && !isClimbing);
        animator.SetBool("isClimbing", isClimbing);

        // --- NEW: Handle Climbing Animation Speed ---
        if (isClimbing)
        {
            // If the player is moving up, down, or sideways on the ladder
            if (verticalInput != 0 || moveInput != 0)
            {
                animator.speed = 1f; // Play animation
            }
            else
            {
                animator.speed = 0f; // Freeze animation
            }
        }
        else
        {
            // Always reset speed to normal when not climbing
            animator.speed = 1f;
        }

        if (moveInput != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);

        if (Input.GetButtonDown("Jump") && isGrounded && !isClimbing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, verticalInput * climbSpeed);
        }
        else
        {
            rb.gravityScale = defaultGravityScale;
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && gunTip != null)
        {
            Instantiate(bulletPrefab, gunTip.position, transform.rotation);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Door"))
        {
            TeleportPlayer();
        }

        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
            // Immediate update for snappier feel
            animator.SetBool("isClimbing", true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
            // Immediate update for snappier feel
            animator.SetBool("isClimbing", false);
        }
    }

    void TeleportPlayer()
    {
        transform.position = new Vector3(25.57f, 4.4f, 0f);
        rb.linearVelocity = Vector2.zero;
        Debug.Log("Player teleported!");
    }
}
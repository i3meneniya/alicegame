using UnityEngine;

public class PlayerMovement : MonoBehaviour

{
    public float speed = 5f;
    public float jumpForce = 5f;
    public bool isGrounded = false;
    public Transform groundcheck;
    public float groundcheckRadius = 0.2f;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    public float fallMultiplier = 3.5f;
    public float lowJumpMultiplier = 3f;
    private float moveInput;
    private bool isJumping = false;
    private float jumpTimecounter;
    public float maxJumpTime = 0.5f;
    public float doubleJumpForce = 4f;
    private bool canDoubleJump = false;
    private float baseSpeed;
    private float baseJumpForce;
    private float baseDoubleJumpForce;
    private float baseGroundcheckRadius;
    private float baseGravityScale;
    private float baseFallMultiplier;
    private float baseLowJumpMultiplier;
    private bool wasGrounded = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        baseSpeed = speed;
        baseJumpForce = jumpForce;
        baseDoubleJumpForce = doubleJumpForce;
        baseGroundcheckRadius = groundcheckRadius;
        baseGravityScale = rb.gravityScale;
        baseFallMultiplier = fallMultiplier;
        baseLowJumpMultiplier = lowJumpMultiplier;
    }
    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
            jumpTimecounter = maxJumpTime;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);   
            canDoubleJump = true;
        }
        
        else if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && canDoubleJump)
        {
            isJumping = true;
            jumpTimecounter = maxJumpTime;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
            canDoubleJump = false;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimecounter > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpTimecounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }

        if (rb.linearVelocity.y < 0 && isJumping)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space) && isJumping)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        if (rb.linearVelocity.y == 0)
        {
            isJumping = false;
        }
    }

    // Scale movement stats proportionally to size factor (1 = original size)
    public void ApplyScale(float scaleFactor)
    {
        speed = baseSpeed * scaleFactor;
        jumpForce = baseJumpForce * scaleFactor;
        doubleJumpForce = baseDoubleJumpForce * scaleFactor;
        groundcheckRadius = baseGroundcheckRadius * scaleFactor;
        rb.gravityScale = baseGravityScale * scaleFactor;
        fallMultiplier = baseFallMultiplier * scaleFactor;
        lowJumpMultiplier = baseLowJumpMultiplier * scaleFactor;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        
        isGrounded = Physics2D.OverlapCircle(groundcheck.position, groundcheckRadius, groundLayer);

        if (isGrounded && !wasGrounded)
        {
            canDoubleJump = false;
        }
        
        wasGrounded = isGrounded;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundcheck.position, groundcheckRadius);
    }
}

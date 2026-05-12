using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int maxJumps = 1;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int jumpCount;
    private float moveInput;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask whatIsGround;

    [Header("Edge/Wall Check")]
    public Transform wallCheck;                 // Точка чуть сбоку от персонажа
    public float wallCheckDistance = 0.2f;      // Радиус проверки касания стены

    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private bool isDashing = false;
    private float lastDashTime;
    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = maxJumps;
    }

    void Update()
    {
        if (isDashing) return;

        moveInput = Input.GetAxisRaw("Horizontal");
        
        // Проверка движения для звуков шагов
        if (Mathf.Abs(rb.velocity.x) > 0.1f)
            isMoving = true;
        else
            isMoving = false;

        // Прыжок
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
        }

        // Рывок
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TryDash();
        }
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        // Направление взгляда
        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // Движение
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Проверка земли и стены
        bool wasGrounded = isGrounded;
        bool groundTouch = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        bool wallTouch = Physics2D.Raycast(wallCheck.position, Vector2.right * transform.localScale.x, wallCheckDistance, whatIsGround);

        isGrounded = groundTouch || wallTouch;

        // Обновляем прыжки при касании земли или небольшой стены
        if (isGrounded && !wasGrounded)
            jumpCount = maxJumps;
    }

    void TryDash()
    {
        if (Time.time < lastDashTime + dashCooldown || isDashing) return;
        if (moveInput == 0) return;

        Vector2 dashDir = moveInput > 0 ? Vector2.right : Vector2.left;
        StartCoroutine(Dash(dashDir));
    }

    IEnumerator Dash(Vector2 direction)
    {
        isDashing = true;
        lastDashTime = Time.time;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = direction * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * transform.localScale.x * wallCheckDistance);
        }
    }

    public bool IsGrounded() => isGrounded;
    public bool IsDashing() => isDashing;
    public bool IsMoving() => isMoving;
}

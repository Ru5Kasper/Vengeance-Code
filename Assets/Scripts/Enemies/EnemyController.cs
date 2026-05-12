using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyShooting))]
public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public Transform groundCheck;
    public Transform wallCheck;
    public float checkDistance = 0.4f;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;

    [Header("Vision Settings")]
    public Transform eyePoint; // точка глаз врага
    public float visionRange = 10f;
    public float visionHeightTolerance = 1.5f;

    [Header("Attack Settings")]
    public float attackRange = 8f;

    private Rigidbody2D rb;
    private Animator anim;
    private Transform player;
    private EnemyShooting shooting;
    private bool isGrounded;
    private bool isDead = false;
    private bool isAttacking = false;
    private int moveDirection = 1; // 1 — вправо, -1 — влево

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        shooting = GetComponent<EnemyShooting>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        
            if (player == null)
        Debug.LogError($"{name}: Игрок не найден! Проверь тег 'Player'.");
    else
        Debug.Log($"{name}: Игрок найден - {player.name}");
    }

    void Update()
    {
        if (isDead || player == null) return;

        CheckGround();

        if (CanSeePlayer())
        {
            FacePlayer();

            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= attackRange)
            {
                rb.velocity = Vector2.zero;
                if (!isAttacking)
                    StartCoroutine(Attack());
            }
            else if (!isAttacking)
            {
                MoveTowardsPlayer();
            }
        }
        else if (!isAttacking)
        {
            Patrol();
        }


    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, whatIsGround);
        Debug.Log($"{name}: isGrounded = {isGrounded}");
    }

    void Patrol()
    {
        // Враг двигается в текущем направлении
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
        Debug.Log($"{name}: Патрулируем, velocity = {rb.velocity}");

        // Точка перед врагом для проверки края
        Vector2 groundCheckPos = (Vector2)groundCheck.position + Vector2.right * moveDirection * 0.3f;

        // Луч вниз от этой точки
        RaycastHit2D groundInfo = Physics2D.Raycast(groundCheckPos, Vector2.down, checkDistance, whatIsGround);
        RaycastHit2D wallInfo = Physics2D.Raycast(wallCheck.position, Vector2.right * moveDirection, checkDistance, whatIsWall);

        // Если впереди нет земли или есть стена — разворот
        if (!groundInfo.collider || wallInfo.collider)
        {
            Flip();
        }
    }


    bool CanSeePlayer()
    {
        if (player == null)
            return false;

        // Используем точку глаз врага
        Vector2 origin = eyePoint != null ? (Vector2)eyePoint.position : (Vector2)transform.position;

        // Проверка расстояния по горизонтали и вертикали
        Vector2 playerPos = player.position;
        float distance = Vector2.Distance(origin, playerPos);
        if (distance > visionRange)
        {
            Debug.Log($"{name}: Игрок слишком далеко ({distance}).");
            return false;
        }

        // Проверка высоты относительно глаз
        float verticalOffset = Mathf.Abs(playerPos.y - origin.y);
        if (verticalOffset > visionHeightTolerance)
        {
            Debug.Log($"{name}: Игрок по высоте вне зоны ({verticalOffset}).");
            return false;
        }

        
        Collider2D col = player.GetComponent<Collider2D>();
        Vector2 playerCenter = col.bounds.center;

        // Raycast к игроку
        Vector2 targetPoint = playerCenter;
        Vector2 directionToPlayer = (targetPoint - origin).normalized;
        RaycastHit2D hit = Physics2D.Raycast(origin, directionToPlayer, visionRange, whatIsGround);


        bool sees = (hit.collider == null || hit.collider.CompareTag("Player"));
        Debug.Log($"{name}: CanSeePlayer() = {sees}, hit = {hit.collider?.name}");
        return sees;
    }


    void FacePlayer()
    {
        if (player == null) return;

        int dir = (player.position.x > transform.position.x) ? 1 : -1;
        if (dir != moveDirection)
        {
            moveDirection = dir;
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * moveDirection;
            transform.localScale = scale;
        }
    }

    void MoveTowardsPlayer()
    {
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
        Debug.Log($"{name}: Двигаемся к игроку, velocity = {rb.velocity}");
    }

    IEnumerator Attack()
    {
            if (player == null)
        {
            Debug.LogWarning($"{name}: Попытка атаковать, но игрок не найден.");
            yield break;
        }

        Debug.Log($"{name}: Атакуем игрока!");
        isAttacking = true;
        anim.SetTrigger("Attack");
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.3f); // задержка перед выстрелом

        shooting.FireAtPlayer(); // <— вызываем стрельбу

        yield return new WaitForSeconds(shooting.attackCooldown);
        isAttacking = false;
    }

    void Flip()
    {
        moveDirection *= -1;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * moveDirection;
        transform.localScale = scale;
    }
    void OnDrawGizmos()
    {
        // Точка глаз
        if (eyePoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(eyePoint.position, 0.1f);
        }

        // Точка игрока
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(player.position, 0.1f);

            // Линия Raycast к игроку
            Vector2 origin = eyePoint != null ? (Vector2)eyePoint.position : (Vector2)transform.position;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(origin, player.position);
        }

        if (eyePoint != null)
        {
            Gizmos.color = new Color(1, 1, 0, 0.3f); // прозрачный желтый
            Vector3 top = eyePoint.position + Vector3.up * visionHeightTolerance;
            Vector3 bottom = eyePoint.position + Vector3.down * visionHeightTolerance;
            Gizmos.DrawLine(new Vector3(transform.position.x - visionRange, top.y, 0), new Vector3(transform.position.x + visionRange, top.y, 0));
            Gizmos.DrawLine(new Vector3(transform.position.x - visionRange, bottom.y, 0), new Vector3(transform.position.x + visionRange, bottom.y, 0));
        }


        // GroundCheck и WallCheck (для патруля)
        if (groundCheck != null)
        {
            Gizmos.color = Color.cyan;
            Vector2 checkPos = (Vector2)groundCheck.position + Vector2.right * moveDirection * 0.3f;
            Gizmos.DrawLine(checkPos, checkPos + Vector2.down * checkDistance);
        }
        if (wallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * moveDirection * checkDistance);
        }
    }


}

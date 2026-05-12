using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerMovement moveScript;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        moveScript = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        float speed = Mathf.Abs(rb.velocity.x);
        bool isJumping = !moveScript.IsGrounded();
        bool isDashing = moveScript.IsDashing();

        // передаём параметры в Animator, не вызываем Play!
        anim.SetFloat("speed", speed);
        anim.SetBool("isJumping", isJumping);
        anim.SetBool("isDashing", isDashing);
    }
}

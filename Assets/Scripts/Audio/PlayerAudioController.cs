using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerAudioController : MonoBehaviour
{
    private PlayerMovement movement;
    private float footstepTimer;
    public float stepInterval = 0.4f;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        HandleFootsteps();
    }

    private void HandleFootsteps()
    {
        if (movement.IsGrounded() && movement.IsMoving())
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                AudioManager.Instance?.PlayFootstep();
                footstepTimer = stepInterval;
            }
        }
    }

    public void PlayShootSound()
    {
        AudioManager.Instance?.PlayShoot();
    }
}

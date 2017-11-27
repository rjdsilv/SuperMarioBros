using UnityEngine;

public class MarioController : MonoBehaviour {
    /// <summary>
    /// Enumeration to indicate Mario's movement direction
    /// </summary>
    private enum MovementDirection
    {
        LEFT,
        RIGHT
    }

    // Constant Declaration.
    const float MAX_JUMP_TIME = 0.40f;

    // Inspector Declaration.
    public float speed = 4;

    // Class Attributes.
    private bool canContinueJump = false;
    private bool isGrounded = false;
    private float jumpTime = 0;
    private Rigidbody2D marioRigidBody2D;
    private Collider2D marioCollider2D;
    private Animator marioAnimator;
    private AudioSource marioJumpSound;
    private MovementDirection currentMovementDirection = MovementDirection.RIGHT;
    private MovementDirection previousMovementDirection = MovementDirection.RIGHT;

    // Use this for initialization
    void Start ()
    {
        marioRigidBody2D = GetComponent<Rigidbody2D>();
        marioCollider2D = GetComponent<Collider2D>();
        marioAnimator = GetComponent<Animator>();
        marioJumpSound = GetComponent<AudioSource>();
    }
	
	// FixedUpdate is called once per frame and will be used for physics
	void FixedUpdate ()
    {
        marioRigidBody2D.velocity = new Vector2(Input.GetAxis("Horizontal"), 0) * speed;

        if (IsJumping())
        {
            marioRigidBody2D.AddForce(transform.up * 30);
        }
        else
        {
            if (!isGrounded)
            {
                marioRigidBody2D.AddForce(transform.up * -25);
            }
        }
    }

    // Detects if the player remains in collision.
    void OnCollisionStay2D(Collision2D collision)
    {
        isGrounded = true;
    }
    
    // Detects if the player exited a collision.
    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    // FixedUpdate is called once per frame and will be used for animations.
    void Update()
    {
        PlayProperAnimationAndSounds();
    }

    /// <summary>
    /// Indicates if Mario is actually moving to the right.
    /// </summary>
    /// <returns>true if moving to the right. false otherwise.</returns>
    private bool IsMovingRight()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            speed = 5;
            previousMovementDirection = currentMovementDirection;
            currentMovementDirection = MovementDirection.RIGHT;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Indicates if Mario is actually moving to the left.
    /// </summary>
    /// <returns>true if moving to the left. false otherwise.</returns>
    private bool IsMovingLeft()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            speed = 5;
            previousMovementDirection = currentMovementDirection;
            currentMovementDirection = MovementDirection.LEFT;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Indicates if Mario is actually jumping.
    /// </summary>
    /// <returns>true if Mario is jumping. false otherwise.</returns>
    private bool IsJumping()
    {
        // Just start a jump if player is on the ground.
        if (isGrounded)
        {
            canContinueJump = true;
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                jumpTime = Time.time;
                return true;
            }
        }

        // If the jump key is not released continue getting up for a maximum ammount of time.
        if ((Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.UpArrow)) && canContinueJump)
        {
            if (Time.time - jumpTime < MAX_JUMP_TIME)
            {
                return true;
            }
        }

        // It the jump key was released, the jump no longer can continue.
        if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            canContinueJump = false;
        }

        return false;
    }

    /// <summary>
    /// Indicates if Mario was previously moving to the right.
    /// </summary>
    /// <returns>true Mario was previously moving to the right. false otherwise.</returns>
    private bool WasMovingRight()
    {
        return previousMovementDirection == MovementDirection.RIGHT;
    }

    /// <summary>
    /// Indicates if Mario was previously moving to the left.
    /// </summary>
    /// <returns>true Mario was previously moving to the left. false otherwise.</returns>
    private bool WasMovingLeft()
    {
        return previousMovementDirection == MovementDirection.LEFT;
    }

    /// <summary>
    /// Indicates if Mario is currently facing right.
    /// </summary>
    /// <returns>true Mario is currently facing right. false otherwise.</returns>
    private bool IsFacingRight()
    {
        return currentMovementDirection == MovementDirection.RIGHT;
    }

    /// <summary>
    /// Indicates if Mario is currently stoped.
    /// </summary>
    /// <returns>true if Mario is stoped. false otherwise.</returns>
    private bool IsStoped()
    {
        bool isStopped = false;

        // Key just released.
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.X))
        {
            isStopped = true;
        }

        // Both left and right pressed at the same time.
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            isStopped = true;
        }

        if (isStopped)
        {
            speed = 0;
            previousMovementDirection = currentMovementDirection;
        }

        return isStopped;
    }

    /// <summary>
    /// Plays Mario's proper animations depending on his current state.
    /// </summary>
    private void PlayProperAnimationAndSounds()
    {
        if (IsJumping())
        {
            // Plays jump sound if not playing already
            if (!marioJumpSound.isPlaying)
            {
                marioJumpSound.Play();
            }

            if (IsFacingRight())
            {
                marioAnimator.Play("MarioJumpingRight");
            }
            else
            {
                marioAnimator.Play("MarioJumpingLeft");
            }
        }
        // Only show walking animation when grounded.
        else if (IsMovingRight() && isGrounded)
        {
            marioAnimator.Play("MarioWalkingRight");
        }
        else if (IsMovingLeft() && isGrounded)
        {
            marioAnimator.Play("MarioWalkingLeft");
        }

        if (IsStoped())
        {
            if (WasMovingRight())
            {
                marioAnimator.Play("MarioIdleRight");
            }
            else if (WasMovingLeft())
            {
                marioAnimator.Play("MarioIdleLeft");
            }
        }
    }
}

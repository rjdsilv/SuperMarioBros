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
    public float speed = 5f;

    // Class Attributes.
    private float jumpTime = 0;
    private Rigidbody2D marioRigidBody2D;
    private Animator marioAnimator;
    private MovementDirection currentMovementDirection = MovementDirection.RIGHT;
    private MovementDirection previousMovementDirection = MovementDirection.RIGHT;

    // Use this for initialization
    void Start ()
    {
        marioRigidBody2D = GetComponent<Rigidbody2D>();
        marioAnimator = GetComponent<Animator>();
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
            marioRigidBody2D.AddForce(transform.up * -25);
        }
    }

    // FixedUpdate is called once per frame and will be used for animations.
    void Update()
    {
        PlayProperAnimation();
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
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpTime = Time.time;
            return true;
        }

        if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.UpArrow))
        {
            if (Time.time - jumpTime < MAX_JUMP_TIME)
            {
                return true;
            }
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
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.X))
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
    private void PlayProperAnimation()
    {
        if (IsJumping())
        {
            if (IsFacingRight())
            {
                marioAnimator.Play("MarioJumpingRight");
            }
            else
            {
                marioAnimator.Play("MarioJumpingLeft");
            }
        }
        else if (IsMovingRight())
        {
            marioAnimator.Play("MarioWalkingRight");
        }
        else if (IsMovingLeft())
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

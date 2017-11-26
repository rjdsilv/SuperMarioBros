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
	
	// Update is called once per frame
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

    void Update()
    {
        PlayProperAnimation();
    }

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

    private bool IsJumping()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            jumpTime = Time.time;
            return true;
        }

        if (Input.GetKey(KeyCode.X))
        {
            if (Time.time - jumpTime < MAX_JUMP_TIME)
            {
                return true;
            }
        }

        return false;
    }

    private bool WasMovingRight()
    {
        return previousMovementDirection == MovementDirection.RIGHT;
    }

    private bool WasMovingLeft()
    {
        return previousMovementDirection == MovementDirection.LEFT;
    }

    private bool IsFacingRight()
    {
        return currentMovementDirection == MovementDirection.RIGHT;
    }

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

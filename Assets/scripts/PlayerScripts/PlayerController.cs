using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LowLevelPhysics2D;

public class PlayerController : MonoBehaviour
{
    //code coyote time and input buffer


    [SerializeField] Rigidbody2D rb;

    #region movement
    public float moveSpeed = 5f;
    [SerializeField] float maxSpeed = 10;
    [SerializeField] float acceleration = 25f;
    [SerializeField] float friction = 10f;
    float HMovement;
    bool isRight = true;
    #endregion

    #region jump
    [SerializeField] float jumpPower = 5;
    float coyoteTime = .3f; //only for jump
    float coyoteTimer;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    #endregion

    
    float buffer = .3f; //for all inputs
    float bufferTimer;

    private void Start()
    {
        Mathf.Clamp(moveSpeed, -maxSpeed, maxSpeed);
    }

    private void FixedUpdate()
    {
        if(HMovement == 0)
        {
            moveSpeed = Mathf.MoveTowards(rb.linearVelocityX, 0, friction * Time.deltaTime);
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocityY);
        }
        else
        {

            flip();
            if (IsGrounded())
            {
                float targetSpeed = maxSpeed * HMovement;
                moveSpeed = Mathf.MoveTowards(rb.linearVelocityX, targetSpeed, acceleration *  Time.deltaTime);
                rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocityY);
            }

        }

        if(IsGrounded())
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }
        
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        HMovement = ctx.ReadValue<Vector2>().x;
        print(HMovement);
    }

    private void flip()
    {
        // checks if player direction has changed, used to flip sprite

        if (isRight == true && HMovement < 0)
        {
            isRight = false;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (isRight == false && HMovement > 0)
        {
            isRight = true;
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            bufferTimer = buffer;
        }
        else
        {
            bufferTimer -= Time.deltaTime;
        }

        if(IsGrounded() && bufferTimer > 0 && coyoteTimer > 0)
        {
            print("jump");
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpPower);
            coyoteTimer = 0;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1f, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    } 


}

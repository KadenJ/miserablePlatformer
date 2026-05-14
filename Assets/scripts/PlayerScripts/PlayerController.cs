using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LowLevelPhysics2D;

public class PlayerController : MonoBehaviour
{

    [SerializeField] Rigidbody2D rb;

    #region movement
    public float moveSpeed = 5f;
    [SerializeField] float friction = 10f;
    float HMovement;
    bool isRight = true;

    [SerializeField] float jumpPower = 5;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    #endregion

    private void FixedUpdate()
    {
        if(HMovement == 0)
        {
            float newH = Mathf.MoveTowards(rb.linearVelocityX, 0, friction * Time.deltaTime);
            rb.linearVelocity = new Vector2(newH, rb.linearVelocityY);
        }
        else
        {
            flip();
            rb.linearVelocity = new Vector2(HMovement * moveSpeed, rb.linearVelocityY);
        }
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        HMovement = ctx.ReadValue<Vector2>().x;  
    }

    private void flip()
    {
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
        print("jump");
        if(ctx.performed && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpPower);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1f, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }


}

using System.Collections.Generic;
using System.Linq;
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
    float buffer = .3f; //for all inputs
    float bufferTimer;
    #endregion


    private void Start()
    {
        Mathf.Clamp(moveSpeed, -maxSpeed, maxSpeed);
    }

    private void FixedUpdate()
    {
        #region movement
        if (HMovement == 0)
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
        #endregion

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

    #region interaction field
    private IInteractables focus = null;
    private List<IInteractables> objectsInRange = new List<IInteractables>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if component has IInteractable, sets interactableInRange to object
        if (collision.TryGetComponent(out IInteractables interactable) && interactable.canInteract)
        {
            //newest object becomes focus
            focus = interactable;
            objectsInRange.Add(interactable);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractables interactable))
        {
            objectsInRange.Remove(interactable);
            if (interactable == focus)
            {
                focus = null;
                if (objectsInRange.Count > 0)
                {
                    focus = objectsInRange.First();
                }
            }
            //search for othe objects in collider and set to interactableInRange
            

        }
    }

    public void interact(InputAction.CallbackContext ctx)
    {
        //objectInFocus.interact
        if (ctx.performed)
        {
            //if interactableInRange exists, call Interact
            focus?.Interact(); 
        }

    }
    #endregion

    private bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1f, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    } 


}

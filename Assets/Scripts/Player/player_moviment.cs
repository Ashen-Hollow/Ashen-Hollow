using System;
using UnityEngine;
using UnityEngine.InputSystem; //import que permite usar o nome input system da unity


public class player_moviment : MonoBehaviour
{
    public PlayerInput playerInput;
    public bool teste = true;
    public Rigidbody2D rb;
    public Animator anim;
    public Animator attackAnim;
    public GameObject slashEffect;

    [Header("Movement Variable")]
    public float velocity;
    public float jumpForce;
    public float jumpCutMultiplier = .9f;
    public float normalGravity;
    public float fallGravity;
    public float jumpGravity;
    private bool isAttacking = false;
    private int facingDirection = 1;




    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public bool isGrounded;


    [Header("Slide Settings")]
    public float slideDuration = .6f;
    private bool sliding;
    private float slideTimer;
    public Vector2 moveInput;
    private bool jumpPressed;
    private bool jumpReleased;




    void Start()
{
    rb.gravityScale = normalGravity;
}

    void Update()
    {
        Flip();
    }

    void FixedUpdate()
    {   
        ApplyVariableGravity();
        CheckGrounded();
        HandleMoviment();
        HandleJump();     
    }

     void ApplyVariableGravity()
    {
        if(rb.linearVelocity.y < -0.3f){
            rb.gravityScale = fallGravity;
        }
        else if(rb.linearVelocity.y > 0.3f)
        {
            rb.gravityScale = jumpGravity;
        }
        else
        {
            rb.gravityScale = normalGravity;
        }
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,groundLayer);
    }
    private void HandleMoviment()
    {
        {
            float targetSpeed = moveInput.x * velocity;
            rb.linearVelocity = new Vector2(targetSpeed,rb.linearVelocity.y);
            if(rb.linearVelocity.x != 0 && isGrounded == true )
            {
                if (isAttacking)
                {
                    rb.linearVelocity = new Vector2(targetSpeed,rb.linearVelocity.y) * 0;
                }
                else
                {
                    anim.SetBool("moving",true);
                    anim.SetBool("isIdle",false);
                    anim.SetBool("jumping",false);
                }
                    
            }
            else if(isGrounded)
            {
                anim.SetBool("isIdle",true);
                anim.SetBool("moving",false);
                anim.SetBool("jumping",false);
            }
        }
        
    }

    private void HandleJump()
    {
        if(jumpPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x,jumpForce);
            jumpPressed = false;
            jumpReleased = false;
            anim.SetBool("isIdle",false);
            anim.SetBool("moving",false);
            anim.SetBool("jumping",true);
        }
        if (jumpReleased)
        {
            if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x,rb.linearVelocity.y * jumpCutMultiplier);
            }
            jumpReleased = false;
        }
    }
    public void SpawnSlash()
{
     slashEffect.SetActive(true);
     attackAnim.SetBool("firstAttack", true);
}

 public void OnAttack(InputValue value)
{

    if (value.isPressed)
    {
                isAttacking = true;
                anim.SetBool("isAttacking", true);
                anim.SetBool("isIdle", false);
                anim.SetBool("jumping",false);
                anim.SetBool("moving",false);
        
    }
}

    public void finishAttack()
    {
        isAttacking = false;
        anim.SetBool("isAttacking",false);
        attackAnim.SetBool("firstAttack",false);
        slashEffect.SetActive(false);
        anim.SetBool("isIdle",true);
    }

    public void OnJump (InputValue value)
    {
         if(value.isPressed)
        {
         jumpPressed = true;
         jumpReleased = false;
        }
        else
        {
            jumpReleased = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color= Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }


    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    

    private void Flip()
    {
        if(moveInput.x < 0 )
        {
            facingDirection = -1;
        }
        else if(moveInput.x > 0)
        {
            facingDirection = 1;
        }

        transform.localScale = new Vector3(facingDirection * 1.6f,1.6f,1);
    }
}

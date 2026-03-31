using System;
using UnityEngine;
using UnityEngine.InputSystem; //import que permite usar o nome input system da unity


public class player_moviment : MonoBehaviour
{
    public GameObject slashEffect;
    public Animator attackAnim;
    public bool teste = true;


    [Header("Components")]
    public PlayerInput playerInput;
    public Rigidbody2D rb;
    public Animator anim;
    public CapsuleCollider2D playerCollider;

    
    [Header("Movement Variable")]
    private bool moving = false;
    private bool idle = false;
    private bool isAttacking = false;
    private bool jumping = false; 
    public float velocity;
    public float jumpForce;
    public float jumpCutMultiplier = .9f;
    public float normalGravity;
    public float fallGravity;
    public float jumpGravity;
    private int facingDirection = 1;
    public Vector2 moveInput;
    private bool jumpPressed;
    private bool jumpReleased;


    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public bool isGrounded;


    [Header("Slide Settings")]
    public float slideDuration = .6f;
    public float slideSpeed = 5;
    public float slideStopDuration = .15f;

    public float slideHeight;
    public Vector2 slideOffset; // allows us to offset the collider so it stays aligned with the bottom of the player
    public float normalHeight;
    public Vector2 normalOffset;

    private bool isSliding;
    private bool slideInputLocked;
    private float slideTimer;
    private float slideStopTimer;
    




    void Start()
{
    rb.gravityScale = normalGravity;
}

    void Update()
    {
        if (!isSliding)
        {
            Flip();
        }
        HandleAnimations();
        HandleSlide();
    }

    void FixedUpdate()
    {   
        ApplyVariableGravity();
        CheckGrounded();

        if (!isSliding)
        {
            HandleMoviment();
        }
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

    void HandleAnimations()
    {
        anim.SetBool("sliding",isSliding && isGrounded);
        anim.SetBool("isAttacking",isAttacking && !isSliding);
        anim.SetBool("isIdle", Math.Abs(moveInput.x) < .1f && isGrounded && !isAttacking && !isSliding);
        anim.SetBool("moving", Math.Abs(moveInput.x) > .1f && isGrounded && !isAttacking && !isSliding);
        anim.SetBool("jumping", isGrounded == false && !isAttacking);
    }

    private void HandleSlide()
        {
            if (isSliding)
            {
                slideTimer -= Time.deltaTime;
                rb.linearVelocity = new Vector2(slideSpeed * facingDirection,rb.linearVelocity.y);

                // If we are done the slide
                if(slideTimer <= 0)
                {
                    isSliding = false;
                    slideStopTimer = slideStopDuration;
                    SetColliderNormal();
                }   
                if(slideStopTimer > 0)
                {
                    slideStopTimer -= Time.deltaTime;
                    rb.linearVelocity = new Vector2(0,rb.linearVelocity.y);
                }
                if(slideStopTimer <= 0 )
                {
                    slideInputLocked = false;
                }
            }
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
        
    }
}

    public void finishAttack()
    {
        isAttacking = false;
        attackAnim.SetBool("firstAttack",false);
        slashEffect.SetActive(false);
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

    public void OnSlide(InputValue value)
    {
        //start the slide
        if (isGrounded && value.isPressed && !isSliding && !slideInputLocked)
        {
            isSliding = true;
            slideInputLocked = true;
            slideTimer = slideDuration;
            SetColliderSlide();
        }
    }

    void SetColliderNormal()
    {
        playerCollider.size = new Vector2(playerCollider.size.x,normalHeight);
        playerCollider.offset = normalOffset;
    }

     void SetColliderSlide()
    {
        playerCollider.size = new Vector2(playerCollider.size.x,slideHeight);
        playerCollider.offset = slideOffset;
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

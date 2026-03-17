using UnityEngine;
using UnityEngine.InputSystem;


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
    private bool isAttacking = false;
    private bool isJumping = false;
    private bool isMoving = false;
    private int facingDirection = 1;
    public Vector2 moveInput;

    void Start()
{
    
}

    // Update is called once per frame
    void FixedUpdate()

    {   
      
        if(isAttacking == true && isMoving == true)
        {
            rb.linearVelocity = new Vector2(moveInput.x,0) * 0;
            anim.SetBool("moving",false);
        }
        else if(isJumping == true && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            isJumping = false;
            anim.SetBool("jumping", false);
            anim.SetBool("isIdle",true);
        }
        else if(isJumping == true || jumpForce != 0)
        {
            rb.linearVelocity = new Vector2(moveInput.x * velocity,jumpForce);
            if(jumpForce > 0){
                jumpForce -= 1;
            }
            anim.SetBool("moving",false);
            anim.SetBool("isIdle",false);
        }
        else
        {
            rb.linearVelocity = new Vector2(moveInput.x * velocity,jumpForce) ;
             if(moveInput.x != 0)
            {
                anim.SetBool("moving",true);
                isMoving = true;
                anim.SetBool("isIdle",false);
             }
            else
            {
                if(isAttacking == false)
                {
                     anim.SetBool("moving",false);
                     isMoving = false;
                     anim.SetBool("isIdle",true);
                }
           
        }
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
                isMoving = false;
                anim.SetBool("isAttacking", true);
                anim.SetBool("isIdle", false);
                anim.SetBool("jumping",false);
                anim.SetBool("moving",false);
        
    }
}

    public void finishAttack()
    {
        isAttacking = false;
        isJumping = false;
        isMoving = false;
        anim.SetBool("isAttacking",false);
        attackAnim.SetBool("firstAttack",false);
        slashEffect.SetActive(false);
        anim.SetBool("isIdle",true);
    }

    public void OnJump (InputValue value)
    {

         if(isJumping != true && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            anim.SetBool("jumping", true);
            anim.SetBool("isIdle",false);
            anim.SetBool("moving",false);
            isMoving = false;
            isJumping = true;
            jumpForce = 12;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x,jumpForce);
            
        }
       

    }


    public void OnMove(InputValue value)
    {
        anim.SetBool("isIdle",false);
        moveInput = value.Get<Vector2>();
        Flip();
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

        transform.localScale = new Vector3(facingDirection,1,1);
    }
}

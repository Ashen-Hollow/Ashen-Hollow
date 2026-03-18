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
    void FixedUpdate()
    {   
        //Caso o jogador comece a atacar e estiver se movendo ele deve parar de se mover e usar o ataque
        if(isAttacking == true && isMoving == true)
        {
            rb.linearVelocity = new Vector2(moveInput.x,0) * 0;
            anim.SetBool("moving",false);
        }
        // caso o jogador não tenha clicado para pular mas por algum motivo esteja em queda
        else if(isJumping == false && Mathf.Abs(rb.linearVelocity.y) > 3.2f && Mathf.Abs(rb.linearVelocity.y)  < 3.4f)
        {
            isJumping = true;
            anim.SetBool("jumping",true);
            anim.SetBool("isIdle",false);
            anim.SetBool("moving",false);
            rb.linearVelocity = new Vector2(moveInput.x * velocity * 0.6f,jumpForce);
        }
        // caso o jogador esteja pulando mas já tenha tocado o chão
        else if(isJumping == true && Mathf.Abs(rb.linearVelocity.y) < 0.2f)
        {
            isJumping = false;
            anim.SetBool("jumping", false);
            anim.SetBool("isIdle",true);
        }
        //caso o usuário ainda esteja no meio do salto a gente reduz a força e impulso dele para iniciar a desaceleração
        else if(isJumping == true || jumpForce != 0)
        {
            rb.linearVelocity = new Vector2(moveInput.x * velocity * 0.8f,jumpForce);
            if(jumpForce > 0){
                jumpForce -= 0.5f;
            }
            anim.SetBool("moving",false);
            anim.SetBool("isIdle",false);
        }
        //caso contrário o jogador estará em movimento ou parado
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
            jumpForce = 11.5f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x* 0.7f,jumpForce);
            
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

        transform.localScale = new Vector3(facingDirection * 1.6f,1.6f,1);
    }
}

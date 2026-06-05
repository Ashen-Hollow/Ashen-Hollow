using System;
using UnityEngine;
using UnityEngine.InputSystem; //import que permite usar o nome input system da unity

public class Player : MonoBehaviour
{

    public PlayerState currentState;
    public PlayerIdleState idleState;
    public PlayerJumpState jumpState;
    public PlayerMoveState moveState;
    public PlayerSlideState slideState;
    public PlayerAttackState attackState;
    public PlayerDamagedState damagedState;
    public GameObject pausePanel;
    public GameObject pauseOverlay;

    public GameObject slashEffect;
    public Animator attackAnim;
    public bool teste = true;
    public Transform pontoDeSpawn;

    


    [Header("Components")]
    public PlayerInput playerInput;
    public Rigidbody2D rb;
    public Animator anim;
    public CapsuleCollider2D playerCollider;

    [Header("Progression Data")]
    public Attributes baseAttributes;
    public int availablePoints;



    public bool attackPressed;



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
    public int facingDirection = 1;


    public Vector2 moveInput;
    public bool jumpPressed;
    public bool jumpReleased;


    [Header("Core Componentes")]
    public Combat combat;
    public Damage damage;
    public Health playerHealth;





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

    public bool isSliding;
    

    private void Awake()
    {
        idleState = new PlayerIdleState(this);
        jumpState = new PlayerJumpState(this);
        moveState = new PlayerMoveState(this);
        slideState = new PlayerSlideState(this);
        attackState = new PlayerAttackState(this);
        damagedState = new PlayerDamagedState(this);
    }

    void Start()
{
    rb.gravityScale = normalGravity;
    ChangeState(idleState);
}

    public void ChangeState(PlayerState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
    }

    void Update()
    {
        currentState.Update();
        if (!isSliding)
        {
            Flip();
        }
        HandleAnimations();
    }

    void FixedUpdate()
    {   
        currentState.FixedUpdate();
        CheckGrounded();
    }

    public void ApplyVariableGravity()
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
        anim.SetBool("isAttacking",isAttacking && !isSliding);
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
                attackPressed = true;
            
    }
}
    

    public void AttackAnimationFinished()
    {
        isAttacking = false;
        attackPressed = false;
        attackAnim.SetBool("firstAttack",false);
        slashEffect.SetActive(false);
        currentState.AttackAnimationFinished();
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
        if (isGrounded && value.isPressed && !isSliding)
        {
            isSliding = true;
            SetColliderSlide();
        }
    }

    public void SetColliderNormal()
    {
        playerCollider.size = new Vector2(playerCollider.size.x,normalHeight);
        playerCollider.offset = normalOffset;
    }

     public void SetColliderSlide()
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

    public void OnPause()
{
    if (UnityEngine.SceneManagement.SceneManager.GetSceneByName("Scenes/UI").isLoaded)
        return;

    bool isPaused = !pausePanel.activeSelf;
    pausePanel.SetActive(isPaused);
    pauseOverlay.SetActive(isPaused);
        
    Time.timeScale = isPaused ? 0f : 1f;
}

    public int GetAtaqueAtual()
    {
        return Stats.AttackDamage(baseAttributes);
    }
    
    public int GetVidaMaximaAtual()
    {
        return Stats.MaxHealth(baseAttributes);
    }

    public int GetDamageDefense()
    {
        return Stats.DamageDefense(baseAttributes);
    }
    public void ResumeGame()
    {
    pausePanel.SetActive(false);
    pauseOverlay.SetActive(false);
    Time.timeScale = 1f;
    
    }    
    public void RestartScene()
{
    Time.timeScale = 1f;
    UnityEngine.SceneManagement.SceneManager.LoadScene(
    UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
    );
}
private void OnDestroy()
    {
        Time.timeScale = 1f; 
        // Adicionado porque o método OnPause 
        // estava travando a cena do menu 
    }

    
}

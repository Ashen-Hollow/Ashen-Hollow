using UnityEngine;

public abstract class PlayerState 
{
    protected Player player;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Damage damage;
    protected Combat combat;

    protected bool JumpPressed {get => player.jumpPressed; set => player.jumpPressed = value;}
    protected bool JumpReleased {get => player.jumpReleased; set => player.jumpReleased = value;} 
    protected bool AttackPressed => player.attackPressed;
    protected Vector2 MoveInput => player.moveInput;
    public PlayerState (Player player)
    {
        this.player = player;
        this.anim = player.anim;
        this.rb = player.rb;
        this.combat = player.combat;
        this.damage = player.damage;
    }

    public virtual void Enter() {}
    public virtual void Exit() {}

    public virtual void Update(){}
    public virtual void FixedUpdate(){}
    public virtual void AttackAnimationFinished(){}
}

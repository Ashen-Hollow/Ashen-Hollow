using UnityEngine;

public class PlayerMoveState : PlayerState
{
    
    public PlayerMoveState(Player player) : base(player){}

    public override void Enter()
    {
        base.Enter();
        anim.SetBool("moving",true);
    }

    public override void Update()
    {
        base.Update();
        if (AttackPressed && combat.CanAttack)
        {
            player.ChangeState(player.attackState);
        }
        else if (JumpPressed)
        {
            player.ChangeState(player.jumpState);
        }
        else if(player.isGrounded && player.isSliding)
        {
            player.ChangeState(player.slideState);
        }
        else if(Mathf.Abs(MoveInput.x) < 0.1f)
        {
            player.ChangeState(player.idleState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        rb.linearVelocity = new Vector2(MoveInput.x * player.velocity,rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        anim.SetBool("moving", false);
        
    }

}

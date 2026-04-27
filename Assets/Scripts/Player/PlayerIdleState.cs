using UnityEngine;

public class PlayerIdleState : PlayerState
{
   public PlayerIdleState(Player player) : base(player){}

   public override void Enter()
    {
        anim.SetBool("isIdle",true);
        rb.linearVelocity = new Vector2(0,rb.linearVelocity.y);
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
            JumpPressed = false;
            player.ChangeState(player.jumpState);
        }
        else if(player.isGrounded && player.isSliding)
        {
            player.ChangeState(player.slideState);
        }
        else if(Mathf.Abs(MoveInput.x) > 0.1f)
        {
            player.ChangeState(player.moveState);
        }
    }

    public override void Exit()
    {
        anim.SetBool("isIdle",false);
    }
  
}

using UnityEngine;

public class PlayerDamagedState : PlayerState
{

    private float timer;
    private float knockbackVelocity;
    private float knockbackDuration;
  
    public PlayerDamagedState(Player player) : base(player){}

    public void SetParameters(int knockbackDirection)
    {
        knockbackVelocity = knockbackDirection * damage.knockbackForce;
    }

    public override void Enter()
    {
        base.Enter();
        anim.SetBool("isDamaged",true);

        knockbackDuration = damage.knockbackDuration;
        player.rb.linearVelocity = new Vector2(knockbackVelocity,player.rb.linearVelocity.y);
    }

    public override void FixedUpdate()
    {
        knockbackDuration -= Time.fixedDeltaTime;
        if(knockbackDuration <= 0)
        {
            player.rb.linearVelocity = Vector2.zero;
            player.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        anim.SetBool("isDamaged",false);
    }

}

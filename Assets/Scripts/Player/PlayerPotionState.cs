using UnityEngine;

public class PlayerPotionState : PlayerState
{
   public PlayerPotionState(Player player) : base(player){}

   public override void Enter()
    {
        anim.SetBool("isPotion",true);
        rb.linearVelocity = new Vector2(0,rb.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        anim.SetBool("isPotion",false);
    }
  
}

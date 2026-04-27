using UnityEngine;

public class PlayerSlideState : PlayerState
{
    private float slideTimer;
    private float slideStopTimer;

    public PlayerSlideState(Player player) : base(player){}

    public override void Enter()
    {
        base.Enter();

        slideTimer = player.slideDuration;
        slideStopTimer = 0;

        player.SetColliderSlide();
        anim.SetBool("sliding",true);
    }

    public override void Update()
    {
        base.Update();

        if(slideTimer > 0)
        {
            slideTimer -= Time.deltaTime;
        }
        else if(slideStopTimer <= 0)
        {
            slideStopTimer = player.slideStopDuration;
        }
        else
        {
            slideStopTimer -= Time.deltaTime;

            if(slideStopTimer <= 0)
            {
                player.ChangeState(player.idleState);
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if(slideTimer > 0)
            rb.linearVelocity = new Vector2(player.slideSpeed * player.facingDirection, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0,rb.linearVelocity.y);

    }

    public override void Exit()
    {
        base.Exit();
        player.isSliding = false;
        player.SetColliderNormal();
        anim.SetBool("sliding",false);
    }


}

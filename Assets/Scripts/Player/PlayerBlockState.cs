using UnityEngine;

public class PlayerBlockState : PlayerState
{
    public bool IsBlocking { get; private set; }

    public PlayerBlockState(Player player) : base(player) { }

    public override void Enter()
    {
        base.Enter();
        IsBlocking = true;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        anim.SetBool("isBlocking", true);
    }

    public override void Update()
    {
        // Solta o botão → sai do block
        if (!player.blockPressed)
        {
            ExitBlock();
            return;
        }

        // Permite andar lentamente enquanto bloqueia
        float slowedSpeed = player.velocity * 0.4f;
        rb.linearVelocity = new Vector2(MoveInput.x * slowedSpeed, rb.linearVelocity.y);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void ExitBlock()
    {
        if (Mathf.Abs(MoveInput.x) > 0.1f)
            player.ChangeState(player.moveState);
        else
            player.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        IsBlocking = false;
        anim.SetBool("isBlocking", false);
    }
}
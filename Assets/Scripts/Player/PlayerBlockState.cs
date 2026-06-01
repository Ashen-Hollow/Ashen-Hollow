using UnityEngine;

public class PlayerBlockState : PlayerState
{
    private float blockDuration;
    private float parryWindow = 0.2f; // janela de tempo para parry perfeito
    private bool isParrying;

    public bool IsBlocking { get; private set; }
    public bool IsParrying => isParrying;

    public PlayerBlockState(Player player) : base(player) { }

    public override void Enter()
    {
        base.Enter();

        IsBlocking = true;
        isParrying = true;          // janela de parry ativa logo ao entrar
        blockDuration = 0f;

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // para o jogador

        anim.SetBool("isBlocking", true);
    }

    public override void Update()
    {
        blockDuration += Time.deltaTime;

        // fecha a janela de parry após parryWindow segundos
        if (isParrying && blockDuration >= parryWindow)
        {
            isParrying = false;
        }

        // sai do block quando o jogador soltar a tecla (blockPressed vai a false em Player.cs)
        if (!player.blockPressed)
        {
            ExitBlock();
            return;
        }

        // permite andar lentamente enquanto bloqueia
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
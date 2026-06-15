using UnityEngine;

public class MeleeAttackState : State
{
    protected override string AnimBoolName => "isAttacking";

    public MeleeAttackState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = Vector2.zero;
    }

    public void TriggerAttack()
    {
        combat.PerformMeleeAttack();
    }

    public override void OnAnimationFinished()
    {
        stateMachine.ChangeState(new ChaseState(enemy));
    }
}

using UnityEngine;

public class RangedAttackState : State
{
    protected override string AnimBoolName => "isShooting";
    private bool attackFinished;

    public RangedAttackState(Enemy enemy) : base(enemy) {}

    public override void Enter()
    {
            base.Enter();

            rb.linearVelocity = Vector3.zero;
            attackFinished = false;
    }

    public override void OnAnimationFinished() => attackFinished = true;

    public override void Update()
    {
        if (!attackFinished)
        {
            return;
        }
        if (senses.GetChaseTarget())
        {
            stateMachine.ChangeState(new ChaseState(enemy));
        }
        else
        {
            stateMachine.ChangeState(new IdleState(enemy));
        }
     }
}

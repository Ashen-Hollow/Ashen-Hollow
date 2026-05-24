using UnityEngine;

public class IdleState : State
{
    private Transform target;
   protected override string AnimBoolName => "isIdle";

    public IdleState(Enemy enemy) : base (enemy){}


    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = Vector2.zero;
    }

    public override void FixedUpdate()
    {
        target = senses.GetChaseTarget();
        if (!target)
        {
            stateMachine.ChangeState(new PatrolState(enemy));
            return;
        }

        enemy.FaceTarget(target);

        if (senses.IsInMeleeRange(target) && combat.CanMeleeAttack())
        {
            stateMachine.ChangeState(new MeleeAttackState(enemy));
            return;
        }

        float distance = Mathf.Abs(target.position.x - enemy.transform.position.x);
        if (distance <= config.turnThreshold)
        {
            // ← REMOVA o return aqui, deixa cair no passo 5
        }

        if (senses.isHittingWall() || senses.isAtCliff())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        stateMachine.ChangeState(new ChaseState(enemy));
    }



}

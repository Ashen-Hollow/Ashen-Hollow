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
        enemy.CurrentTarget = target;
        
        if (!target && enemy.Config.patrolSpeed != 0)
        {
            stateMachine.ChangeState(new PatrolState(enemy));
            return;
        }

        if(enemy.Config.patrolSpeed == 0 && target)
        {
            return;
        }

        enemy.FaceTarget(target);

        if (senses.IsInMeleeRange(target) && combat.CanMeleeAttack())
        {
            stateMachine.ChangeState(new MeleeAttackState(enemy));
            return;
        }

        if (senses.IsInShootingRange(target) && combat.CanRangedAttack())
        {
            stateMachine.ChangeState(new RangedAttackState(enemy));
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

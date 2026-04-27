using UnityEngine;

public abstract class State 
{
    protected Rigidbody2D rb;
    protected Animator anim;
    protected virtual string AnimBoolName => null;
    protected EnemyConfig config;
    protected Enemy_Senses senses;
    protected Enemy_Combat combat;
    protected Enemy enemy;
    protected StateMachine stateMachine;


    protected State(Enemy enemy)
    {
        rb = enemy.RB;
        anim = enemy.Anim;
        config = enemy.Config;
        senses = enemy.Senses;
        stateMachine = enemy.StateMachine;
        combat = enemy.Combat;
        this.enemy = enemy;

    }

    public virtual void Enter()
    {
        if(!string.IsNullOrEmpty(AnimBoolName))
            anim.SetBool(AnimBoolName,true);
    }

    public virtual void Update(){}
    public virtual void FixedUpdate(){}
    public virtual void OnAnimationFinished(){}
    public virtual void Exit()
    {
        if(!string.IsNullOrEmpty(AnimBoolName))
            anim.SetBool(AnimBoolName,false);
    }
}

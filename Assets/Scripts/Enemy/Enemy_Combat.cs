using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;

    private EnemyConfig config;
    private Enemy enemy;
    private float lastAttackTime;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        config = enemy.Config; 
    }

    public bool CanMeleeAttack() => Time.time >= lastAttackTime + config.meleeCoolDown;

   public void PerformMeleeAttack()
    {
        lastAttackTime = Time.time;
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position,config.meleeRange); 

        if(!hit)
            return;

        Health health = hit.GetComponent<Health>();
        if(health != null)
            health.ChangeHealth(-config.meleeDamage,transform.position);
    }
}

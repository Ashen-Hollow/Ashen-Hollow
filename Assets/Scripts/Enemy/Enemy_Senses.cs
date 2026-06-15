using UnityEngine;

public class Enemy_Senses : MonoBehaviour
{   
    [SerializeField] private Enemy enemy;
    [SerializeField] private EnemyConfig config;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform[] wallChecks;
    [SerializeField] private Transform attackPoint;


    public bool isAtCliff() => !Physics2D.Raycast(groundCheck.position,Vector2.down,config.groundCheckDistance,config.groundLayer); 

    public bool isHittingWall(){
        Vector2 dir = Vector2.right * enemy.FacingDirection;
        foreach(Transform check in wallChecks)
        {
            bool hitWall = Physics2D.Raycast(check.position,dir,config.wallCheckDistance,config.wallLayer);
            if (hitWall)
            {
                return true;
            }
        }
            return false;
        
        }
         
    public Transform GetChaseTarget()
    {
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position,config.chaseRange,config.targetLayer);
        if(!hit)
            return null;
        return hit.transform;
    }

    public bool IsInMeleeRange(Transform target)
    {
        if(!target)
            return false;

        float distance = Vector2.Distance(target.position, attackPoint.position);
        return distance <= config.meleeRange;
    }

    public bool IsInShootingRange(Transform target)
    {
        if(!target)
            return false;

        float distance = Vector2.Distance(target.position, attackPoint.position);
        return distance <= config.rangedRange;
    }

    public void OnDrawGizmosSelected()
    {
        //Ground Check
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(groundCheck.position,groundCheck.position + Vector3.down * config.groundCheckDistance);

        //Wall Check
        Gizmos.color = Color.blue;
        Vector3 dir = Vector3.right * enemy.FacingDirection;
        foreach(Transform check in wallChecks)
            Gizmos.DrawLine(check.position, check.position + dir * config.wallCheckDistance);

        //Chase Check
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position,config.chaseRange);

        //Melee Check
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(attackPoint.position, config.meleeRange);

        //Ranged Check
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, config.rangedRange);
    }

}

using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
 [Header("General")]
 public float turnThreshold = .2f;
 

 [Header("Patrol")]
 public float  patrolSpeed = 2;
 public float groundCheckDistance = .7f;
 public float wallCheckDistance = .5f;
 public LayerMask groundLayer;
 public LayerMask wallLayer;


 [Header("Chase")]
 public float chaseSpeed = 2.5f;
 public float chaseRange = 2;
 public LayerMask targetLayer;

 [Header("Attack")]
 public float meleeRange = .8f;
 public int meleeDamage = 30;
 public float meleeCoolDown = 1;

[Header("Ranged Attack")]
public float rangedRange = 5;
public int rangedDamage = 1;
public float rangedCooldown = 2;
public GameObject projectilePrefab;
public float projectileSpeed = 12;
public float projectileLifeTime = 3;
public float aimHeightOffset = 0.5f;




 [Header("Damaged")]
 public float knockbackDuration = .2f;
 public float knockbackForce = 10;


}

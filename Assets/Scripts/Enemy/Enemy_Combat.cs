using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;

    private EnemyConfig config;
    private Enemy enemy;
    private float lastAttackTime;

    private Player player;
    private int playerDefense;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        config = enemy.Config; 
        if (player == null)
    {
        player = FindFirstObjectByType<Player>(); // Na Unity antiga use FindObjectOfType<Player>()
        playerDefense = player.GetDamageDefense();
        print(playerDefense);
    }
    }

    public bool CanMeleeAttack() => Time.time >= lastAttackTime + config.meleeCoolDown;

    public bool CanRangedAttack() => Time.time >= lastAttackTime + config.rangedCooldown;

   public void PerformMeleeAttack()
    {
        lastAttackTime = Time.time;
    
    // 1. Pega todos os colisores dentro do raio de ataque
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, config.meleeRange); 

    // 2. Percorre a lista de objetos encontrados
        foreach (Collider2D hit in hits)
        {
        // 3. Verifica se o objeto atual da lista tem a tag "Player"
            if (hit.CompareTag("Player"))
            {
                Health health = hit.GetComponent<Health>();
            
                if (health != null)
                {
                    health.ChangeHealth(-config.meleeDamage + playerDefense, transform.position);
                    break; 
                }
            }
        }
    }

    public void PerformRangedAttack()
{
    lastAttackTime = Time.time;

    // Aponta para um ponto mais alto do alvo (ajuste o 0.5f conforme necessário)
    Vector3 aimTarget = enemy.CurrentTarget.position + new Vector3(0, 0.5f, 0);
    Vector2 fireDirection = (aimTarget - attackPoint.position).normalized;

    float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
    Quaternion rotation = Quaternion.Euler(0, 0, angle);

    GameObject newProjectile = Instantiate(config.projectilePrefab, attackPoint.position, rotation);
    ProjectTile projectile = newProjectile.GetComponent<ProjectTile>();
    projectile.Damage = config.rangedDamage;
    projectile.Lifetime = config.projectileLifeTime;

    Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
    rb.linearVelocity = fireDirection * config.projectileSpeed;
}
}

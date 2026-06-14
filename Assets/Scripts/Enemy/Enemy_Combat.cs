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
    }
    }

    public bool CanMeleeAttack() => Time.time >= lastAttackTime + config.meleeCoolDown;

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
}

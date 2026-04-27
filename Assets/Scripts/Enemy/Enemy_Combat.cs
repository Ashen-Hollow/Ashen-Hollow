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
    
    // 1. Pega todos os colisores dentro do raio de ataque
    Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, config.meleeRange); 

    // 2. Percorre a lista de objetos encontrados
    foreach (Collider2D hit in hits)
    {
        // 3. Verifica se o objeto atual da lista tem a tag "Player"
        if (hit.CompareTag("Player"))
        {
            Health health = hit.GetComponent<Health>();
            
            // 4. Aplica o dano se o componente Health existir
            if (health != null)
            {
                health.ChangeHealth(-config.meleeDamage, transform.position);
                
                // Se você quiser que o inimigo pare de atacar após acertar o player 
                // (mesmo que haja outros objetos), use 'break'.
                // Se quiser dar dano em tudo que for player no raio, não use break.
                break; 
            }
        }
    }
}
}

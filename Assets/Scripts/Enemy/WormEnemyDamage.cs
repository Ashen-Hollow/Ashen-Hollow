using UnityEngine;

/// <summary>
/// Substitui o EnemyDamage padrão na minhoca.
/// Ao morrer, tem chance de explodir e causar dano em área no jogador.
/// </summary>
public class WormEnemyDamage : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Enemy enemy;
    public Health health;

    [Header("Death FX (partes espalhadas)")]
    [SerializeField] private GameObject[] deathParts;
    [SerializeField] private float spawnForce = 5f;
    [SerializeField] private float torque = 5f;
    [SerializeField] private float lifeTime = 2f;

    [Header("Explosão")]
    [Tooltip("Chance de explodir ao morrer (0 = nunca, 1 = sempre)")]
    [Range(0f, 1f)]
    [SerializeField] private float explosionChance = 0.4f;

    [SerializeField] private int explosionDamage = 2;
    [SerializeField] private float explosionRadius = 1.5f;
    [SerializeField] private GameObject explosionVFXPrefab; // opcional — arraste um prefab de partícula
    [SerializeField] private LayerMask playerLayer;

    // ??? Eventos de vida ??????????????????????????????????????????????????????

    private void OnEnable()
    {
        health.OnDamage += HandleDamage;
        health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        health.OnDamage -= HandleDamage;
        health.OnDeath -= HandleDeath;
    }

    // ??? Handlers ?????????????????????????????????????????????????????????????

    void HandleDamage(Vector2 sourcePosition)
    {
        int knockbackDir = transform.position.x > sourcePosition.x ? 1 : -1;
        enemy.StateMachine.ChangeState(new DamagedState(enemy, knockbackDir));
    }

    void HandleDeath()
    {
        SpawnDeathParts();

        if (Random.value <= explosionChance)
            Explode();

        Destroy(gameObject);
    }

    // ??? Lógica de explosão ???????????????????????????????????????????????????

    private void Explode()
    {
        // VFX (opcional — só instancia se o prefab foi atribuído no Inspector)
        if (explosionVFXPrefab != null)
        {
            GameObject vfx = Instantiate(explosionVFXPrefab, transform.position, Quaternion.identity);
            Destroy(vfx, 2f);
        }

        // Detecta o jogador dentro do raio
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, playerLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Health playerHealth = hit.GetComponent<Health>();
                if (playerHealth != null)
                    playerHealth.ChangeHealth(-explosionDamage, transform.position);
            }
        }
    }

    // ??? Partes de morte (igual ao EnemyDamage original) ?????????????????????

    private void SpawnDeathParts()
    {
        foreach (GameObject prefab in deathParts)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0.5f, 1f)).normalized;
            GameObject part = Instantiate(prefab, transform.position, rotation);

            Rigidbody2D rb = part.GetComponent<Rigidbody2D>();
            Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;
            rb.linearVelocity = randomDirection * spawnForce;
            rb.AddTorque(Random.Range(-torque, torque), ForceMode2D.Impulse);

            Destroy(part, lifeTime);
        }
    }

    // ??? Gizmo (visualiza o raio no Editor) ??????????????????????????????????

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.3f, 0f, 0.4f);
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}
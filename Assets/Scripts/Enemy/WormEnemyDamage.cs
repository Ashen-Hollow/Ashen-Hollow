using System.Collections;
using UnityEngine;

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
    [SerializeField] private GameObject explosionVFXPrefab;
    [SerializeField] private LayerMask playerLayer;

    [Header("Aviso Visual (Piscar tipo Creeper)")]
    [Tooltip("Quantas vezes pisca antes de explodir")]
    [SerializeField] private int flashCount = 5;
    [Tooltip("Intervalo entre cada piscada (segundos)")]
    [SerializeField] private float flashInterval = 0.15f;
    [Tooltip("Cor do flash de aviso")]
    [SerializeField] private Color warningColor = Color.red;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    // ─── Lifecycle ──────────────────────────────────────────────────────────────

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

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

    // ─── Handlers ───────────────────────────────────────────────────────────────

    void HandleDamage(Vector2 sourcePosition)
    {
        int knockbackDir = transform.position.x > sourcePosition.x ? 1 : -1;
        enemy.StateMachine.ChangeState(new DamagedState(enemy, knockbackDir));
    }

    void HandleDeath()
    {
        SpawnDeathParts();

        if (Random.value <= explosionChance)
            StartCoroutine(ExplodeWithWarning());
        else
            Destroy(gameObject);
    }

    // ─── Aviso visual + Explosão ─────────────────────────────────────────────────

    private IEnumerator ExplodeWithWarning()
    {
        // Desabilita componentes de comportamento para a minhoca ficar parada
        // durante o aviso visual (opcional — remova se quiser que ela continue se movendo)
        if (enemy != null)
            enemy.enabled = false;

        // Pisca como o Creeper
        for (int i = 0; i < flashCount; i++)
        {
            // Acende
            if (spriteRenderer != null)
                spriteRenderer.color = warningColor;

            yield return new WaitForSeconds(flashInterval);

            // Apaga
            if (spriteRenderer != null)
                spriteRenderer.color = originalColor;

            yield return new WaitForSeconds(flashInterval);
        }

        // Explode de verdade
        Explode();
        Destroy(gameObject);
    }

    private void Explode()
    {
        // VFX (opcional — instancia se o prefab foi atribuído no Inspector)
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

    // ─── Partes de morte (igual ao EnemyDamage original) ────────────────────────

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

    // ─── Gizmo (visualiza o raio no Editor) ──────────────────────────────────────

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.3f, 0f, 0.4f);
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}
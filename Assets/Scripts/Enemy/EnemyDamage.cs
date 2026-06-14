using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    public Animator anim;
    public Health health;

    [Header("Death FX")]
    [SerializeField] private GameObject[] deathParts;
    [SerializeField] private float spawnForce = 5;
    [SerializeField] private float torque = 5;
    [SerializeField] private float lifeTime = 2;


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

    void HandleDamage(Vector2 sourcePosition)
    {
        int knockbackDir = 0;
        knockbackDir = transform.position.x > sourcePosition.x ? 1 : -1;

        enemy.StateMachine.ChangeState(new DamagedState(enemy,knockbackDir));
    }

    void HandleDeath()
    {

        if(deathParts.Length == 0 && CompareTag("Flying_Demon"))
        {
            enemy.Anim.Play("Death");
            Destroy(gameObject, 1);
            return;
        }

        foreach(GameObject prefab in deathParts)
        {
            Quaternion rotation = Quaternion.Euler(0,0,Random.Range(0.5f,1)).normalized;
            GameObject part = Instantiate(prefab, transform.position,rotation);

            Rigidbody2D rb = part.GetComponent<Rigidbody2D>();

            Vector2 randomDirection = new Vector2(Random.Range(-1,1),Random.Range(.5f,1)).normalized;
            rb.linearVelocity = randomDirection * spawnForce;
            rb.AddTorque(Random.Range(-torque,torque),ForceMode2D.Impulse);

            Destroy(part,lifeTime);

        }
        Destroy(gameObject);
    }
}

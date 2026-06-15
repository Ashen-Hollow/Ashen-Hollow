using UnityEngine;

public class ProjectTile : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask targetLayer;

    public float Lifetime {get;set;} = 5;
    public int Damage {get; set;} = 1;

    void Start()
    {
        Destroy(gameObject, Lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Destroy(gameObject);
            return;
        }

        if(((1<<collision.gameObject.layer) & targetLayer) == 0)
        {
            return;
        }

        Health health = collision.GetComponentInChildren<Health>();

        if (health)
        {
            health.ChangeHealth(-Damage, transform.position);
            Destroy(gameObject);
        }
    }
}

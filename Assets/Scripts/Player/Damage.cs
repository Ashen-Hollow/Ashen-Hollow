using UnityEngine;
using UnityEngine.SceneManagement;
public class Damage : MonoBehaviour
{
  [SerializeField] private Player player;
   public Health health;

   [Header("KnockBack Settings")]
   public float knockbackForce = 5;
   public float knockbackDuration = .2f;
   public GameObject uiController;

   
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
        if (player.currentState is PlayerBlockState blockState)
        {
            if (blockState.IsParrying)
            {
                Debug.Log("Parry! Nenhum dano recebido.");
                return;
            }
            else
            {
                Debug.Log("Block! Dano absorvido.");
                uiController.GetComponent<HeartControl>().UpdateHearts(health.health);
                return;
            }
        }

        int knockbackDir = transform.position.x > sourcePosition.x ? 1 : -1;
        player.damagedState.SetParameters(knockbackDir);
        player.ChangeState(player.damagedState);
        uiController.GetComponent<HeartControl>().UpdateHearts(health.health);
    }

    void HandleDeath()
    {
        SceneManager.LoadScene("GameOver");
    }
}

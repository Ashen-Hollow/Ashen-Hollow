using UnityEngine;
using System;

public class Health : MonoBehaviour
{

    public event Action<int, int> OnHealthChanged;
    public event Action<Vector2> OnDamage;
    public event Action OnDeath;

    public int health;
    public int maxHealth;

    public Player player;

    [SerializeField] private HeartControl heartControl;

    public void Start()
    {
        if (player != null)
        {
            maxHealth = player.GetVidaMaximaAtual();
        }

        health = maxHealth;
        OnHealthChanged?.Invoke(health, maxHealth);


        if (heartControl != null)
            heartControl.UpdateHearts(health);
    }

    public void ChangeHealth(int amount, Vector2 sourcePosition)
    {
        health += amount;
        print(amount);

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        OnHealthChanged?.Invoke(health,maxHealth);

        if (heartControl != null)
            heartControl.UpdateHearts(health);

        if (health <= 0)
        {
            OnDeath?.Invoke();
        }
        else if (amount < 0)
        {
            OnDamage?.Invoke(sourcePosition);
        }
    }
}
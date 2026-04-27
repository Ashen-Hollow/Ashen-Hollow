using UnityEngine;
using System;

public class HealthDummy : MonoBehaviour
{
   public event Action OnDamage;
   public event Action OnDeath;
   public int health;
   public int maxHealth;


   public void Start()
    {
        health = maxHealth;
    }

   public void ChangeHealth(int amount)
    {
        health += amount;

        if(health > maxHealth)
        {
            health = maxHealth;
        }
        else if(health <= 0)
        {
            OnDeath?.Invoke();
            
        }
        else if(amount < 0)
        {
            OnDamage?.Invoke();
            
        }
    }
   
}

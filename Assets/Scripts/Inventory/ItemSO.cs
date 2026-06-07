using UnityEngine;

[CreateAssetMenu(fileName = "Novo Item", menuName = "Inventario/Item Ativavel")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange;
    public int amountToChangeStat;

    // Recebe o alvo (o Player)
    public void UseItem(GameObject target)
    {
        if (statToChange == StatToChange.health)
        {
            // Busca o script Health no Player
            Health targetHealth = target.GetComponent<Health>();

            if (targetHealth != null)
            {
                // Manda curar a vida. Usamos Vector2.zero porque não há empurrão/dano na cura
                targetHealth.ChangeHealth(amountToChangeStat, Vector2.zero);
                Debug.Log($"Usou {itemName}! Curou {amountToChangeStat} de HP.");
            }
            else
            {
                Debug.LogWarning("Script Health não encontrado no alvo!");
            }
        }
    }

    public enum StatToChange
    {
        none,
        health,
        mana,
        stamina
    }
}
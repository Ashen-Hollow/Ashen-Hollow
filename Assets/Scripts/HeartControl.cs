using UnityEngine;
using UnityEngine.UI; // Necessário para manipular Image

public class HeartControl : MonoBehaviour
{
    // Arraste as 3 imagens de coração para este array no Inspector
    [SerializeField] private Image[] hearts; 

    /// <summary>
    /// Define quantos corações devem estar visíveis.
    /// </summary>
    /// <param name="health">Quantidade de vida atual (ex: 0 a 3)</param>
    public void UpdateHearts(int health)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            // Se o índice atual for menor que a vida, mostra o coração.
            // Caso contrário, esconde.
            hearts[i].enabled = i < health;

            // Dica: Você também pode usar .gameObject.SetActive(i < health) 
            // se preferir que o objeto suma completamente da hierarquia.
        }
    }
}
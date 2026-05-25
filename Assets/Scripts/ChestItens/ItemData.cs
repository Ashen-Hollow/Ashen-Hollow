using UnityEngine;

[CreateAssetMenu(fileName = "NovoItem", menuName = "Inventario/Item")]
public class ItemData : CollectibleSO
{
    // Como o CollectibleSO tem um método abstrato, somos obrigados a implementá-lo aqui
    public override void Collect(Player player)
    {
        Debug.Log("Você coletou: " + itemName);
        // Depois você coloca a lógica de dar dinheiro, curar vida, etc.
    }
}
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private string itemName;

    [SerializeField]
    private int quantity;

    [SerializeField]
    private Sprite sprite;

    private InventoryManager inventoryManager;

    void Start()
    {
        // NOVA LINHA: Esse comando mágico acha o inventário mesmo se ele estiver desligado!
        inventoryManager = FindFirstObjectByType<InventoryManager>(FindObjectsInactive.Include);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Confirmação extra de segurança para evitar erros
            if (inventoryManager != null)
            {
                inventoryManager.AddItem(itemName, quantity, sprite);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("InventoryManager não encontrado na cena!");
            }
        }
    }
}
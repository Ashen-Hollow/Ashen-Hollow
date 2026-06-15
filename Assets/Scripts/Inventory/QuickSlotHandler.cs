using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QuickSlotHandler : MonoBehaviour
{
    public Image quickSlotImage; // Este é o seu 'ItemIcon' (o filho)
    public InventoryManager inventoryManager;
    private string currentItemName;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame)
        {
            if (!string.IsNullOrEmpty(currentItemName))
            {
                // Tenta consumir
                inventoryManager.UseEquippedItem();
            }
        }
    }

    public void SetQuickSlot(string itemName, Sprite itemSprite)
    {
        currentItemName = itemName;
        if (quickSlotImage != null)
        {
            quickSlotImage.sprite = itemSprite;
            quickSlotImage.enabled = true; // Mostra a poção
        }
    }

    // --- NOVO MÉTODO ---
    public void ClearQuickSlot()
    {
        currentItemName = ""; // Limpa a memória do nome
        if (quickSlotImage != null)
        {
            quickSlotImage.enabled = false; // Esconde a poção, o fundo permanece!
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;
    public ItemSlot[] itemSlot;

    void Start()
    {
        if (InventoryMenu != null)
        {
            menuActivated = false;
            InventoryMenu.SetActive(false);
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame)
        {
            menuActivated = !menuActivated;

            InventoryMenu.SetActive(menuActivated);

            if (menuActivated)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            // CORREÇÃO 1: Trocado 'name' por 'itemName' e adicionado parênteses na lógica.
            // A lógica agora é: "Se o slot não está cheio E (é o mesmo item OU está vazio)"
            if (itemSlot[i].isFull == false && (itemSlot[i].itemName == itemName || itemSlot[i].quantity == 0))
            {
                int leftOverItems = itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription);

                if (leftOverItems > 0)
                {
                    // CORREÇÃO 2 e 3: Trocado '==' por '=' e adicionado o ';' no final
                    leftOverItems = AddItem(itemName, leftOverItems, itemSprite, itemDescription);
                }

                return leftOverItems;
            }
        }
        return quantity;
    }

    public void DeselecteAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
    }
}
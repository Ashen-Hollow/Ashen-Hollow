using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // Necessário para a Image do atalho
using TMPro; // Necessário para o Texto do atalho

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;
    public ItemSlot[] itemSlot;

    public ItemSO[] itemSOs;
    public GameObject player;

    // --- ADICIONADO: Variáveis da Interface do Atalho ---
    [Header("Atalho Rapido (Quick Slot)")]
    public Image quickSlotImage;
    public TMP_Text quickSlotQuantityText;
    public Sprite emptyQuickSlotSprite;
    private string equippedItemName;

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

        // --- ADICIONADO: Tecla 'C' para usar o item do atalho durante o jogo ---
        if (Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame)
        {
            if (!menuActivated) // Só usa se o menu estiver fechado
            {
                UseEquippedItem();
            }
        }
    }

    public void UseItem(string itemName)
    {
        for (int i = 0; i < itemSOs.Length; i++)
        {
            if (itemSOs[i].itemName == itemName)
            {
                itemSOs[i].UseItem(player);
                return;
            }
        }

        Debug.LogWarning("O item " + itemName + " não foi encontrado no banco de dados do InventoryManager!");
    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].isFull == false && (itemSlot[i].itemName == itemName || itemSlot[i].quantity == 0))
            {
                int leftOverItems = itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription);

                if (leftOverItems > 0)
                {
                    leftOverItems = AddItem(itemName, leftOverItems, itemSprite, itemDescription);
                }

                // --- ADICIONADO: Atualiza o número do atalho se você coletar mais do mesmo item ---
                UpdateQuickSlotUI();

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

    // =======================================================
    // --- ADICIONADO: TODAS AS FUNÇÕES DO ATALHO RÁPIDO ---
    // =======================================================

    public void EquipQuickItem(string itemName, Sprite itemSprite)
    {
        equippedItemName = itemName;
        if (quickSlotImage != null)
        {
            quickSlotImage.sprite = itemSprite;
            quickSlotImage.enabled = true;
        }
        UpdateQuickSlotUI(); // Atualiza a quantidade ao equipar
    }

    public void UseEquippedItem()
    {
        if (string.IsNullOrEmpty(equippedItemName)) return;

        // Procura no inventário o slot que tem o item equipado
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].quantity > 0 && itemSlot[i].itemName == equippedItemName)
            {
                UseItem(equippedItemName); // Manda o efeito curar a vida
                itemSlot[i].ConsumeOne(); // Gasta 1 unidade do inventário de forma segura
                UpdateQuickSlotUI(); // Atualiza a tela do atalho
                return; // Sai para não gastar 2 poções ao mesmo tempo
            }
        }
    }

    public void UpdateQuickSlotUI()
    {
        // 1. Procura se existe alguma poção desse tipo no inventário
        int totalQuantity = 0;
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].quantity > 0 && itemSlot[i].itemName == equippedItemName)
            {
                totalQuantity += itemSlot[i].quantity;
            }
        }

        // 2. Se tem poções, atualiza o ícone e o texto
        if (totalQuantity > 0)
        {
            if (quickSlotQuantityText != null)
            {
                quickSlotQuantityText.text = totalQuantity.ToString();
                quickSlotQuantityText.enabled = true;
            }
            // LIGA a imagem do ícone (o "ItemIcon" filho)
            if (quickSlotImage != null)
            {
                quickSlotImage.enabled = true;
            }
        }
        else
        {
            // 3. SE ACABOU, apenas esconde o ícone e o texto.
            // O fundo fixo (QuickSlotUI) continuará lá, intacto.
            if (quickSlotQuantityText != null)
            {
                quickSlotQuantityText.text = "";
                quickSlotQuantityText.enabled = false;
            }

            // DESLIGA a imagem do ícone. O fundo, que é outro objeto, 
            // não é afetado, então nunca ficará branco!
            if (quickSlotImage != null)
            {
                quickSlotImage.enabled = false;
            }
        }
    }


}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;

    [SerializeField] private int maxNumberOfItems;

    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;

    public Image itemDescriptionImage;
    public TMP_Text ItemDescriptionNameText;
    public TMP_Text ItemDescriptionText;

    public GameObject selectedShader;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;

    public void Start()
    {
        inventoryManager = FindFirstObjectByType<InventoryManager>(FindObjectsInactive.Include);
    }

    // Mantido o SEU código original que funciona perfeitamente para empilhar
    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        if (isFull)
            return quantity;

        this.itemName = itemName;
        this.itemSprite = itemSprite;
        this.itemDescription = itemDescription;

        itemImage.sprite = this.itemSprite;
        itemImage.enabled = true;

        this.quantity += quantity;

        if (this.quantity >= maxNumberOfItems)
        {
            quantityText.text = maxNumberOfItems.ToString();
            quantityText.enabled = true;
            isFull = true;

            int extraItems = this.quantity - maxNumberOfItems;
            this.quantity = maxNumberOfItems;
            return extraItems;
        }

        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;

        return 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    public void OnLeftClick()
    {
        // O DUPLO CLIQUE: Se o slot já estava selecionado E tem item dentro, ele usa!
        if (thisItemSelected && quantity > 0)
        {
            inventoryManager.UseItem(itemName);

            quantity -= 1;
            quantityText.text = quantity.ToString();

            // Se o item acabou, limpa a tela e sai da função
            if (quantity <= 0)
            {
                EmptySlot();
                return;
            }
        }

        // O PRIMEIRO CLIQUE (ou clique normal): Mantido o seu código original
        inventoryManager.DeselecteAllSlots();
        selectedShader.SetActive(true);
        thisItemSelected = true;

        ItemDescriptionNameText.text = itemName;
        ItemDescriptionText.text = itemDescription;
        itemDescriptionImage.sprite = itemSprite;

        if (itemDescriptionImage.sprite == null)
        {
            itemDescriptionImage.sprite = emptySprite;
        }
    }

    public void OnRightClick()
    {
        // Lógica de usar ou dropar o item no futuro!
    }

    // Função que limpa os dados quando o item acaba
    private void EmptySlot()
    {
        quantity = 0;
        isFull = false;
        itemName = "";
        itemDescription = "";

        itemImage.sprite = emptySprite;
        itemImage.enabled = false;
        quantityText.enabled = false;

        ItemDescriptionNameText.text = "";
        ItemDescriptionText.text = "";
        itemDescriptionImage.sprite = emptySprite;

        inventoryManager.DeselecteAllSlots();
    }
}
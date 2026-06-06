using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;

    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;

    public void AddItem(string itemName, int quantity, Sprite itemSprite)
    {
        this.itemName = itemName;
        this.quantity = quantity;
        this.itemSprite = itemSprite;
        isFull = true;

        quantityText.text = quantity.ToString();
        quantityText.enabled = true;

        // Passa a imagem para o componente UI
        itemImage.sprite = itemSprite;

        // LIGA O COMPONENTE DE IMAGEM
        itemImage.enabled = true;
    }
}
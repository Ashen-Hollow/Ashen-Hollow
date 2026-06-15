using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class AttributesSlot : MonoBehaviour
{
    public Attributes.AttributeType type;

    [Header("UI Elements")]
    public TMP_Text valueText;
    public Button increaseButton;
    public Button decreaseButton;
    public Slider slider;

    public ProgressionManager manager;

    public void Setup(ProgressionManager manager)
    {
        this.manager = manager;
        //Setup increase and decrease button
        increaseButton.onClick.AddListener(() => manager.ModifyAttribute(type, 1));
        decreaseButton.onClick.AddListener(() => manager.ModifyAttribute(type, -1));
    }

    public void Refresh()
    {
        int previewValue = manager.GetAttributeValue(type,true);
        valueText.text = previewValue.ToString();

        slider.maxValue = 20;
        slider.value = previewValue;
    }
}

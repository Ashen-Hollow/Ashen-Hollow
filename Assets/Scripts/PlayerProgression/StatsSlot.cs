using UnityEngine;
using TMPro;

public class StatsSlot : MonoBehaviour
{
    
    public string displayName;
    public TMP_Text nameText;
    public TMP_Text valueText;
    public TMP_Text previewText;

    private void Awake()
    {
        nameText.text = displayName;

    }

    public void Refresh(float baseValue, float previewValue)
    {
        valueText.text = previewValue.ToString();
        float diff = previewValue - baseValue;

        if(diff > 0)
        {
            previewText.text = "+" + diff.ToString();
        }
        else if(diff < 0)
        {
            previewText.text = "-" + diff.ToString();
        }
        else
        {
            previewText.text = "";
        }
    }
}

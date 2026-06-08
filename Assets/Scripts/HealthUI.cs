using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    public Slider slider;
    public TMP_Text healthText;
    public Health health;

    private void OnEnable() 
    {
        health.OnHealthChanged += UpdateUI;
    
    }


    private void OnDisable()
    {
        health.OnHealthChanged -= UpdateUI;

    }



    private void UpdateUI(int current, int max)
    {
        slider.maxValue = max;
        slider.value = current;

        healthText.text = $"{current} / {max}";
    }
}

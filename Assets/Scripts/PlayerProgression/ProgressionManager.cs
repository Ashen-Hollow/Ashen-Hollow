using UnityEngine;
using TMPro;

public class ProgressionManager : MonoBehaviour
{
  
  [Header ("UI References")]
  public TMP_Text pointsText;
  //Slot Reference
  public AttributesSlot[] attributesSlots;
  public StatsSlot[] statsSlot;

  [Header ("Settings")]
  [HideInInspector] public int availablePoints;
  private int startingPoints; 

  [HideInInspector] public Attributes baseAttributes; 
  [HideInInspector] public Attributes previewAttributes; 

  private Player playerScript;

  private void Start()
    {
        
        playerScript = FindFirstObjectByType<Player>(); 

        if (playerScript != null)
        {
            baseAttributes = playerScript.baseAttributes;
            availablePoints = playerScript.availablePoints;
        }
        else
        {
            Debug.LogError("ProgressionManager: Não foi possível encontrar o script do Player na cena!");
        }

        startingPoints = availablePoints;
        previewAttributes = baseAttributes.Clone();

        foreach(AttributesSlot slot in attributesSlots)
        {
            slot.Setup(this);
        }

        RefreshUI();
    }

    public int GetAttributeValue(Attributes.AttributeType type, bool isPreview)
    {
        return isPreview? previewAttributes.Get(type) : baseAttributes.Get(type);
    }

    public void ModifyAttribute(Attributes.AttributeType type, int amount)
    {
        int currentPreview = previewAttributes.Get(type);

        if(amount > 0 && availablePoints <= 0) //Stop overspending
        {
            return;
        }

       int permanentBase = baseAttributes.Get(type);
       if(amount < 0 && currentPreview <= permanentBase)
        {
            return;
        }

        previewAttributes.Set(type, currentPreview + amount);
        availablePoints -= amount;

        RefreshUI();

    }

    public void ConfirmChanges()
    {
        baseAttributes = previewAttributes.Clone();
        startingPoints = availablePoints;

        if (playerScript != null)
        {
            playerScript.baseAttributes = baseAttributes.Clone();
            playerScript.availablePoints = availablePoints;

        }

        RefreshUI();
    }

    public void CancelChanges()
    {
        //Reset preview back to base and restore points
        previewAttributes = baseAttributes.Clone();
        availablePoints = startingPoints;
        RefreshUI();
    }

    private void RefreshUI()
    {
        pointsText.text = availablePoints.ToString();
        //Slot refresh
        foreach(AttributesSlot slot in attributesSlots)
        {
            slot.Refresh();
        }
        RefreshStatsMenu();
    }

    public void RefreshStatsMenu()
    {
        statsSlot[0].Refresh(
            Stats.AttackDamage(baseAttributes),
            Stats.AttackDamage(previewAttributes));

        statsSlot[1].Refresh(
            Stats.DamageDefense(baseAttributes),
            Stats.DamageDefense(previewAttributes));

        statsSlot[2].Refresh(
            Stats.MaxHealth(baseAttributes),
            Stats.MaxHealth(previewAttributes));
    }


}

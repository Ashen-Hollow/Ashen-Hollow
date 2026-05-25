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
  public int availablePoints = 5;
  private int startingPoints; //Tracks the starting value in case we cancel and need to reset

  public Attributes baseAttributes; //The "Real" stats
  public Attributes previewAttributes; //The "Drafts" stats

  private void Start()
    {
        //Initialize
        startingPoints = availablePoints;
        previewAttributes = baseAttributes.Clone();

        //Setup our slots
        foreach(AttributesSlot slot in attributesSlots)
        {
            slot.Setup(this);
                
            
        }

        //Setup our slots
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
        //Overwrite our base attributes with preview values
        baseAttributes = previewAttributes.Clone();
        startingPoints = availablePoints;
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

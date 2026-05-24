using UnityEngine;

public abstract class CollectibleSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public Sprite itemSprite;

    public abstract void Collect(Player player);

}

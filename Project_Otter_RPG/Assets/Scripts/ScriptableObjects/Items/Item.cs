using UnityEngine;

public abstract class Item : ScriptableObject
{
    public enum ItemType
    {
        DEFAULT,
        HEALING,
        DAMAGING
    }

    public ItemType itemType = ItemType.DEFAULT;

    public string itemName;

}

using UnityEngine;

public abstract class Item : ScriptableObject
{
    public enum ItemType
    {
        DEFAULT,
        HEALING,
        DAMAGING
    }

    public string itemName;

}

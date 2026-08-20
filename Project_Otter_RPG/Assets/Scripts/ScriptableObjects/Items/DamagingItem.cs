using UnityEngine;

[CreateAssetMenu(fileName = "DamagingItem", menuName = "Scriptable Objects/DamagingItem")]
public class DamagingItem : Item
{
    public int damage;

    public ItemType itemType = ItemType.HEALING;
}

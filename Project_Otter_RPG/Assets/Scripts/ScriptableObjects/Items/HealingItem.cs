using UnityEngine;

[CreateAssetMenu(fileName = "HealingItem", menuName = "Scriptable Objects/ItemData")]
public class HealingItem : Item
{
    public int healAmount;

    public ItemType itemType = ItemType.HEALING;
}

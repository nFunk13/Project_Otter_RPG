using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Inventory : MonoBehaviour
{
    public class ItemData
    {
        public Item identifier;
        public int amount;

        public void init(Item ident, int startNumber)
        {
            identifier = ident;
            amount = startNumber;
        }
    }

    Dictionary<string, ItemData> inventory;

    public void AddItem(GameObject obj)
    {
        if (obj.tag == "Item")
        {
            if (obj.TryGetComponent(out ItemDataHolder itemData))
            {
                if (inventory.ContainsKey(itemData.itemData.itemName) && inventory[itemData.itemData.itemName].amount < 99)
                {
                    inventory.TryGetValue(itemData.itemData.itemName, out ItemData inventoryItem);
                    inventoryItem.amount++;
                }
                else
                {
                    ItemData newItem = new ItemData();
                    newItem.init(obj.GetComponent<ItemDataHolder>().itemData, 1);
                    inventory.Add(obj.GetComponent<ItemDataHolder>().itemData.itemName, newItem);
                }
            }
        }
    }
}

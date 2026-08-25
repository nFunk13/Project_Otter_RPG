using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Rendering;

public class Inventory : MonoBehaviour
{
    // Class for item data to be used for the inventory
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

    /// <summary>
    /// Adds an item to the inventory either by increasing the amount or by adding a new item to the dictionary
    /// </summary>
    /// <param name="obj"></param> The gameobject that contains the item data
    public void AddItem(GameObject obj)
    {
        if (obj.tag == "Item")
        {
            if (obj.TryGetComponent(out ItemDataHolder itemData))
            {
                // Checks if the item is already in the inventory
                if (inventory.ContainsKey(itemData.itemData.itemName) && inventory[itemData.itemData.itemName].amount < 99)
                {
                    // Increases the amount by 1 if the item amount is less that 99
                    inventory.TryGetValue(itemData.itemData.itemName, out ItemData inventoryItem);
                    inventoryItem.amount++;
                }
                else
                {
                    // Adds a new item into the inventory
                    ItemData newItem = new ItemData();
                    newItem.init(obj.GetComponent<ItemDataHolder>().itemData, 1);
                    AddNewItem(newItem);
                }
            }
        }

        Destroy(obj);
    }

    private void AddNewItem(ItemData newItemData)
    {
        int inventoryIndex = 0;
        bool itemAdded = false;
        string newItemName = newItemData.identifier.itemName;
        List<ItemData> healingItems = new List<ItemData>();
        List<ItemData> attackItems = new List<ItemData>();

        // Puts items into respective lists for healing or attack
        foreach (var item in inventory.Values)
        {
            if (item.identifier.itemType == Item.ItemType.HEALING)
            {
                healingItems.Add(item);
            }
            else if (item.identifier.itemType == Item.ItemType.DAMAGING)
            {
                attackItems.Add(item);
            }
        }

        // Checks what type of item the new item is, healing or attack
        if (newItemData.identifier.itemType == Item.ItemType.HEALING)
        {
            foreach (var item in healingItems)
            {
                // Goes through each character in the item name string
                for (int i = 0; i < item.identifier.itemName.Length; i++)
                {
                    if (newItemName[i] == newItemName[newItemName.Length - 1])
                    {
                        // Inserts the new item before the current item in the list if the char is smaller
                        healingItems.Insert(i, item);
                        itemAdded = true;
                        break;
                    }
                    else if (item.identifier.itemName[i] == item.identifier.itemName[item.identifier.itemName.Length - 1])
                    {
                        // moves to the next item
                        break;
                    }
                    else if (newItemName[i] > item.identifier.itemName[i])
                    {
                        // Inserts the item before the current item
                        healingItems.Insert(i, item);
                        itemAdded = true;
                        break;
                    }
                    inventoryIndex++;
                }
                if (itemAdded)
                {
                    break;
                }
            }
        }
        else if (newItemData.identifier.itemType == Item.ItemType.DAMAGING)
        {
            foreach (var item in attackItems)
            {
                // Goes through each character in the item name string
                for (int i = 0; i < item.identifier.itemName.Length; i++)
                {
                    if (newItemName[i] == newItemName[newItemName.Length - 1])
                    {
                        // Inserts the new item before the current item in the list if the char is smaller
                        attackItems.Insert(i, item);
                        itemAdded = true;
                        break;
                    }
                    else if (item.identifier.itemName[i] == item.identifier.itemName[item.identifier.itemName.Length - 1])
                    {
                        // moves to the next item
                        //inventoryIndex++;
                        //attackItems.Insert(i, item);
                        //itemAdded = true;
                        break;
                    }
                    else if (newItemName[i] > item.identifier.itemName[i])
                    {
                        // Inserts the item before the current item
                        attackItems.Insert(i, item);
                        itemAdded = true;
                        break;
                    }
                    inventoryIndex++;
                }
                if (itemAdded)
                {
                    break;
                }
            }
        }

        // Resets the invenotry
        inventory = new Dictionary<string, ItemData>();

        // Adds the new item order of healing items
        foreach (var item in healingItems)
        {
            inventory.Add(item.identifier.itemName, item);
        }
        // Adds the new item order of attack items
        foreach (var item in attackItems)
        {
            inventory.Add(item.identifier.itemName, item);
        }
    }

    /// <summary>
    /// Called when an item needs to be removed from the inventory
    /// This can be a decrease of the amount or removed from the inventory as a whole
    /// </summary>
    /// <param name="obj"></param> The game object that the player is trying to interact with
    public void RemoveItem(GameObject obj)
    {
        // Checks if the object is tagged as an item
        if (obj.tag == "Item")
        {
            // Checks to make sure that the object has an ItemDataHolder script
            if (obj.TryGetComponent<ItemDataHolder>(out ItemDataHolder itemDataHolder))
            {
                // If the item has more than one in the inventory, decrease the amount by 1
                if (inventory[itemDataHolder.itemData.itemName].amount > 1)
                {
                    inventory[itemDataHolder.itemData.itemName].amount--;
                }
                // If the item only has one for the amount in the inventory, remove the item from the inventory
                else if (inventory[itemDataHolder.itemData.itemName].amount == 1)
                {
                    inventory.Remove(itemDataHolder.itemData.itemName);
                }
            }
        }
    }
}

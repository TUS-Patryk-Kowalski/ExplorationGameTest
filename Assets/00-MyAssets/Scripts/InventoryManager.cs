using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public bool freeSlotsAvailable = true;

    public List<InventoryItemData> _toolbarSlots = new List<InventoryItemData>();

    public List<InventoryItemData> inventorySlots = new List<InventoryItemData>();
    public void AddItemToInventory(Item itemToAdd)
    {
        // Create a new InventoryItemData
        InventoryItemData newItemData = new InventoryItemData();

        // Assuming the Item class has a property or a field that refers to an ItemSO
        newItemData.itemInSlot = itemToAdd.itemSO; // Replace 'itemSO' with the actual name of the property/field in your Item class that refers to an ItemSO

        // Add the quantity from itemToAdd to the newItemData
        newItemData.AddToSlot(itemToAdd.quantityInStack);

        // Add the newItemData to the inventorySlots
        inventorySlots.Add(newItemData);

        // Destroy the itemToAdd gameObject
        Destroy(itemToAdd.gameObject);
    }

    public void CheckForFreeSlots()
    {
        foreach (InventoryItemData inventorySlot in inventorySlots)
        {
            // check each slot to see if it is free
            // if slot is free and not in free slot list, add index to free slot list
            // if slot is not free and in the free slot list, remove its idex from the list
            // Suggestion 
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public void AddItemToInventory(Item itemToAdd)
    {
        CheckForFreeSlots();

        // If there are free slots available
        if (GameManager.Instance.freeSlotsAvailable)
        {
            ItemSO itemSO = itemToAdd.GetComponent<Item>().itemSO;
            if (!itemSO.isStackable)
            {
                foreach (InventoryItemData inventorySlot in GameManager.Instance.inventorySlots)
                {
                    if (inventorySlot.itemInSlot == null)
                    {
                        inventorySlot.itemInSlot = itemSO;
                        // not working at the moment
                        Debug.Log("function called");
                        Destroy(itemToAdd.gameObject);
                        break;
                    }
                }

                CheckForFreeSlots(); // or directly remove the index of the slot from the list
            }
            else
            {
                foreach (InventoryItemData inventorySlot in GameManager.Instance.inventorySlots)
                {
                    // Look for the item in the inventory
                    if (inventorySlot.itemInSlot == itemSO)
                    {
                        // if found increnemt the count by amount in the ground stack
                        inventorySlot.AddToSlot(itemToAdd.quantityInStack);
                        break;
                    }
                    // if not found, add it to a free slot
                }

                CheckForFreeSlots(); // or directly remove the index of the slot from the list
            }
        }
    }

    public void CheckForFreeSlots()
    {
        foreach (InventoryItemData inventorySlot in GameManager.Instance.inventorySlots)
        {
            // check each slot to see if it is free
            // if slot is free and not in free slot list, add index to free slot list
            // if slot is not free and in the free slot list, remove its idex from the list
            // Suggestion 
        }
    }
}

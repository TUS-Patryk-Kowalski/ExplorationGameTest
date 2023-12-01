using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItemData
{
    public ItemSO itemInSlot;
    private int quantity;

    public void AddToSlot(int amount)
    {
        quantity += amount;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player;

    public GameObject cameraPoint;

    public Sprite missingSprite;
    public Color common, uncommon, rare, epic, legendary, mythic, otherworldly;

    private List<InventoryItemData> _toolbarSlots = new List<InventoryItemData>();
    private List<InventoryItemData> _inventorySlots = new List<InventoryItemData>();
    private bool _freeSlotsAvailable;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
            
        player = GameObject.FindWithTag("Player");
        cameraPoint = GameObject.Find("PlayerFollowCamera");
    }

    public void AddItemToInventory(ItemSO itemSOToAdd, Item itemToAdd)
    {
        CheckForFreeSlots();

        // If there are free slots available
        if (_freeSlotsAvailable)
        {
            if (!itemSOToAdd.isStackable)
            {
                foreach (InventoryItemData inventorySlot in _inventorySlots)
                {
                    if (inventorySlot.itemInSlot == null)
                        inventorySlot.itemInSlot = itemSOToAdd;
                        break;
                }

                CheckForFreeSlots(); // or directly remove the index of the slot from the list
            }
            else
            {
                foreach (InventoryItemData inventorySlot in _inventorySlots)
                {
                    // Look for the item in the inventory
                    if (inventorySlot.itemInSlot == itemSOToAdd)
                    {
                        // if found increnemt the count by amount in the ground stack
                        inventorySlot.AddToSlot(itemToAdd.quantityOnGround);
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
        foreach(InventoryItemData inventorySlot in _inventorySlots)
        {
            // check each slot to see if it is free
            // if slot is free and not in free slot list, add index to free slot list
            // if slot is not free and in the free slot list, remove its idex from the list
            // Suggestion 
        }
    }
}

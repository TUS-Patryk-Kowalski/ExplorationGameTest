using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItemData
{
    public ItemSO itemInSlot;
    private int quantity;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player;

    public GameObject cameraPoint;

    public Sprite missingSprite;
    public Color common, uncommon, rare, epic, legendary, mythic, otherworldly;

    private List<InventoryItemData> _inventorySlots = new List<InventoryItemData>();
    private bool _freeSlotsAvailable;
    private List<int> _freeSlotIndexes;

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

    public void AddItemToInventory(ItemSO itemToAdd)
    {
        CheckForFreeSlots();

        if (_freeSlotsAvailable)
        {
            if (!itemToAdd.isStackable)
            {
                // Add the item to a free inventory slot
                CheckForFreeSlots(); // or directly remove the index of the slot from the list
            }
            else
            {
                // Look through inventory list to see if the item is there,
                // if found increnemt the count by 1
                // if not found, add it to a free slot
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

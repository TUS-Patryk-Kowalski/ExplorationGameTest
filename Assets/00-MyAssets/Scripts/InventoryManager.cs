using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItemData
{
    public ItemSO itemInSlot;
    [SerializeField]
    private int quantity;

    public void AddToSlot(int amount)
    {
        quantity += amount;
    }
}

public class InventoryManager : MonoBehaviour
{
    public int startingInventorySlots, startingHotbarSlots;

    public List<GameObject> hotbarSlotGO = new List<GameObject>();
    public List<InventoryItemData> hotbarSlots = new List<InventoryItemData>();
    public List<GameObject> inventorySlotGO = new List<GameObject>();
    public List<InventoryItemData> inventorySlots = new List<InventoryItemData>();

    private void Awake()
    {
        for (int i = 0; i < startingInventorySlots; i++)
        {
            inventorySlots.Add(new InventoryItemData());
        }

        for (int i = 0; i < startingHotbarSlots; i++)
        {
            hotbarSlots.Add(new InventoryItemData());
        }
    }

    private void Start()
    {
        GetSlots();
    }

    private void GetSlots()
    {
        foreach (Transform child in GameManager.Instance.hotbar.transform)
        {
            hotbarSlotGO.Add(child.gameObject);
        }
    }
    public void AddItemToInventory(Item itemToAdd)
    {
        if (!itemToAdd.stackable)
        {
            AddNonStackableItem(itemToAdd);
        }
        else
        {
            AddStackableItem(itemToAdd);
        }
    }

    private void AddNonStackableItem(Item itemToAdd)
    {
        foreach (InventoryItemData slot in inventorySlots)
        {
            if (slot.itemInSlot == null)
            {
                slot.itemInSlot = itemToAdd.itemSO;
                slot.AddToSlot(itemToAdd.quantityInStack);
                Destroy(itemToAdd.gameObject);
                return;
            }
        }
        // Handle full inventory scenario
    }

    private void AddStackableItem(Item itemToAdd)
    {
        foreach (InventoryItemData slot in inventorySlots)
        {
            if (slot.itemInSlot != null && slot.itemInSlot == itemToAdd.itemSO && slot.itemInSlot.itemRarity == itemToAdd.itemSO.itemRarity)
            {
                slot.AddToSlot(itemToAdd.quantityInStack);
                Destroy(itemToAdd.gameObject);
                return;
            }
        }

        // If item not found in inventory, add to first empty slot
        foreach (InventoryItemData slot in inventorySlots)
        {
            if (slot.itemInSlot == null)
            {
                slot.itemInSlot = itemToAdd.itemSO;
                slot.AddToSlot(itemToAdd.quantityInStack);
                Destroy(itemToAdd.gameObject);
                return;
            }
        }
        // Handle full inventory scenario
    }

    private void UpdateUI()
    {
        foreach (GameObject itemSlot in GameManager.Instance.inventoryManager.inventorySlotGO)
        {
            
        }
    }
}

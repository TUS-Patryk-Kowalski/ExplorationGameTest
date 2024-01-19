using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Common.Enums;

[Serializable]
public class InventoryItemData
{
    public GameObject itemSlotGO;

    [SerializeField]
    private ItemSO _itemInSlot;

    public ItemSO itemInSlot
    {
        get { return _itemInSlot; }
    }

    [SerializeField]
    private int _quantity;

    public int quantity
    {
        get { return _quantity; }
    }

    public void AddToSlot(Item item)
    {
        if(item.itemSO) _itemInSlot = item.itemSO;
        if(item.itemSO) _quantity += item.quantityInStack;
        GameObject.Destroy(item.gameObject);
    }
}

[Serializable]
public class RaritySpritePair
{
    public Rarity rarity;
    public Sprite sprite;
}

public class InventoryManager : MonoBehaviour
{
    public int startingInventorySlots;
    public GameObject inventorySlotPrefab;

    public List<InventoryItemData> hotbarSlots = new List<InventoryItemData>();
    public List<RaritySpritePair> rarityToSprite = new List<RaritySpritePair>();

    private void Start()
    {
        for (int i = 0; i < startingInventorySlots; i++)
        {
            Instantiate(inventorySlotPrefab, GameManager.Instance.inventoryUI.transform);
            hotbarSlots.Add(new InventoryItemData());
            hotbarSlots[i].itemSlotGO = GameManager.Instance.inventoryUI.transform.GetChild(i).gameObject;
        }
        // Add a hotbar selection display object to the
    }

    public void AddItemToInventory(Item itemToAdd)
    {
        UpdatedPickup(itemToAdd);
        UpdateUI();
    }

    private void UpdatedPickup(Item itemToAdd)
    {
        foreach (InventoryItemData slot in hotbarSlots)
        {
            if (slot.itemInSlot != null && slot.itemInSlot == itemToAdd.itemSO && slot.itemInSlot.itemRarity == itemToAdd.itemSO.itemRarity && slot.quantity < itemToAdd.settingsSO.maxStack)
            {
                slot.AddToSlot(itemToAdd);
                return;
            }
        }

        // If item not found in inventory, add to first empty slot
        foreach (InventoryItemData slot in hotbarSlots)
        {
            if (slot.itemInSlot == null)
            {
                slot.AddToSlot(itemToAdd);
                return;
            }
        }
    }

    private void UpdateUI()
    {
        foreach (InventoryItemData slot in hotbarSlots)
        {
            if (slot.itemInSlot != null)
            {
                slot.itemSlotGO.transform.GetChild(0).GetComponent<Image>().sprite = slot.itemInSlot.itemSprite;

                foreach (RaritySpritePair rarityToSprite in rarityToSprite)
                {
                    if (slot.itemInSlot.itemRarity == rarityToSprite.rarity)
                    {
                        slot.itemSlotGO.GetComponent<Image>().sprite = rarityToSprite.sprite;
                        break;
                    }
                }
            }
        }
    }
}

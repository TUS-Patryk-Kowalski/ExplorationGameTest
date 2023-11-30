using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemRarity
{
    NotSet = 0,
    Common = 1,
    Uncommon = 2,
    Rare = 3,
    Epic = 4,
    Legendary = 5,
    Mythic = 6,
    Otherworldly = 7
}

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Items/Generic")]
public class ItemSO : ScriptableObject
{
    [Header("Item Details")]
    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
    public ItemRarity itemRarity;
    [Space]
    [Header("Item Settings")]
    public bool isStackable;
    public float itemMass;
}

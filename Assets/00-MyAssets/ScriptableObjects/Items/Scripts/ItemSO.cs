using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Enums;



[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Items/Generic")]
public class ItemSO : ScriptableObject
{
    [Header("Item Details")]
    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
    public Rarity itemRarity;
    [Space]
    [Header("Item Settings")]
    public ItemSettingsSO itemSettingsSO;
}

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

    [Space]
    [Header("Object references")]
    public GameObject player;
    public GameObject cameraPoint;

    public Vector3 cameraDirection;

    [Space]
    [Header("Defaults in case of errors")]
    public Sprite missingSprite;

    [Space]
    [Header("Item Rarity Colours")]
    public Color common;
    public Color uncommon;
    public Color rare;
    public Color epic;
    public Color legendary;
    public Color mythic;
    public Color otherworldly;

    [Space]
    [Header("Global Item Settings")]
    public ItemSettingsSO settingsSO;

    [Space]
    [Header("Inventory")]
    public bool freeSlotsAvailable;
    [SerializeField]
    private List<InventoryItemData> _toolbarSlots = new List<InventoryItemData>();
    [SerializeField]
    private List<InventoryItemData> _inventorySlots = new List<InventoryItemData>();
    public List<InventoryItemData> inventorySlots 
    {
        get { return _inventorySlots; }
    }

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

    private void Update()
    {
        cameraDirection = player.transform.eulerAngles;
    }
    // if something fails on an item, use this function-
    // to make the object easier to locate in-game
    public IEnumerator FlashRed(Light light)
    {
        while (true)
        {
            light.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            light.color = Color.black;
            yield return new WaitForSeconds(0.2f);
        }
    }
}

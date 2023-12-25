using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    namespace Enums
    {
        public enum Rarity
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
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Space]
    [Header("Object references")]
    public GameObject player;
    public GameObject cameraPoint;
    public GameObject inventoryUI;

    [Space]
    [Header("Component references")]
    public InventoryManager inventoryManager;

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

    private Vector3 _cameraDirection;
    public Vector3 cameraDirection
    {
        get { return _cameraDirection; }
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
        inventoryUI = GameObject.Find("HotbarHUD");
        inventoryManager = gameObject.GetComponent<InventoryManager>();
    }

    private void Update()
    {
        _cameraDirection = player.transform.eulerAngles;
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

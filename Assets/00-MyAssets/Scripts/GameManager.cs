using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public List<DungeonRoomSet> DungeonPrefabSets = new List<DungeonRoomSet>();

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
            
        if(!player) player = GameObject.FindWithTag("Player");
        if(!cameraPoint) cameraPoint = GameObject.Find("PlayerFollowCamera");
        if(!inventoryUI) inventoryUI = GameObject.Find("HotbarHUD");
        if(!inventoryManager) inventoryManager = gameObject.GetComponent<InventoryManager>();
    }

    private void Update()
    {
        _cameraDirection = player.transform.eulerAngles;
    }

    // if something fails on an item, use this function-
    // to make the object easier to locate in-game
    public IEnumerator Flash(Light light, Color color)
    {
        while (true)
        {
            light.color = color;
            yield return new WaitForSeconds(0.2f);
            light.color = Color.black;
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void MovePlayer(Transform newPosition, Vector3 offset)
    {
        player.SetActive(false);
        player.GetComponentInParent<Transform>().position = (newPosition.position + offset);
        player.transform.position = (newPosition.position + offset);
        player.SetActive(true);
    }
}

[Serializable]
public class DungeonRoomSet
{
    public List<GameObject> StartingRooms = new List<GameObject>();
    public List<GameObject> NormalRooms = new List<GameObject>();
    public List<GameObject> Corridors = new List<GameObject>();
    public List<GameObject> BonusRooms = new List<GameObject>();
    public List<GameObject> BossRooms = new List<GameObject>();
    public List<GameObject> ReturnPortalRoom = new List<GameObject>();
}

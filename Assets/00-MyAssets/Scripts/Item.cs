using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemSO itemSO;
    public ItemSettingsSO settingsSO;
    [Space]
    private Collider _itemCollider;
    private Rigidbody _itemRB;
    private SpriteRenderer _itemSR;
    private Light _itemLight;
    [Space]
    public float bobbingHeight = 0.2f;
    public float bobbingSpeed = 2f, bobbingSpeedOffset = 0.5f;
    [Space]
    private int _quantityInStack = 1;
    public int quantityInStack
    {
        get { return _quantityInStack; }
    }

    public bool stackable;

    private void OnEnable()
    {
        // 1. Get components
        _itemCollider = GetComponent<Collider>();
        _itemRB = GetComponent<Rigidbody>();
        _itemSR = GetComponentInChildren<SpriteRenderer>();
        _itemLight = GetComponentInChildren<Light>();

        // 2. Set the variables
        originalY = _itemSR.transform.localPosition.y;
        bobbingSpeed = Random.Range(bobbingSpeed - bobbingSpeedOffset, bobbingSpeed + bobbingSpeedOffset);
        _itemRB.mass = itemSO.itemMass;
        if(_itemRB.mass < 0.25f) _itemRB.mass = 0.25f;

        // Since this is in the OnEnable function-
        // this makes sure the starting intensity doesn't get set again-
        // when the item leaves the player's range and comes back
        if (startingLightIntensity == 0) startingLightIntensity = _itemLight.intensity;

        // Check if ScriptableObject is assigned
        if (itemSO == null)
        {
            Debug.LogWarning($"ScriptableObject is missing on the item {gameObject.name}!");
            _itemSR.sprite = GameManager.Instance.missingSprite;
            StartCoroutine(GameManager.Instance.FlashRed(_itemLight));
            return;
        }

        // Set the sprite                  if true set itemSO's sprite | if false get fallback sprite
        _itemSR.sprite = itemSO.itemSprite != null ? itemSO.itemSprite : GameManager.Instance.missingSprite;

        // If itemSO is assigned but has no sprite variable assigned... do:
        if (itemSO.itemSprite == null)
        {
            Debug.LogWarning($"Item sprite is missing in the ScriptableObject for the item: {gameObject.name}!");
            StartCoroutine(GameManager.Instance.FlashRed(_itemLight));
        }

        SetLightColourBasedOnRarity();
    }

    private void FixedUpdate()
    {
        PerformDistanceCulling();
        if (spriteEnabled) { SpriteFacePlayer(); BobbingMotion();}
        if (lightEnabled) { AdjustLightIntensity();}
    }

    // Trun the sprite of the item to always face the camera
    private void SpriteFacePlayer()
    {
        _itemSR.transform.eulerAngles = GameManager.Instance.cameraDirection;
    }

    // The item sprite bobs up and down slowly
    private float originalY;
    private void BobbingMotion()
    {
        // Calculate the new Y position
        float newY = originalY + bobbingHeight * Mathf.Sin(Time.time * bobbingSpeed);

        // Update the localPosition of the item
        _itemSR.transform.localPosition = new Vector3(0, newY, 0);
    }

    // for performance reasons, make different components of an item get disabled-
    // once the player gets a specified distance away
    private bool spriteEnabled;
    private bool lightEnabled;
    private bool physicsEnabled;
    private void PerformDistanceCulling()
    {
        float distanceToPlayer = Vector3.Distance(GameManager.Instance.player.transform.position, transform.position);

        spriteEnabled = distanceToPlayer <= GameManager.Instance.settingsSO.spriteRenderDistance;
        lightEnabled = distanceToPlayer <= GameManager.Instance.settingsSO.lightRenderDistance;
        physicsEnabled = distanceToPlayer <= GameManager.Instance.settingsSO.physicsActiveDistance;

        if (_itemSR != null) _itemSR.gameObject.SetActive(spriteEnabled);
        if (_itemLight != null) _itemLight.gameObject.SetActive(lightEnabled);
        if (_itemRB != null) _itemRB.isKinematic = !physicsEnabled;
        if (_itemCollider != null) _itemCollider.enabled = physicsEnabled;
    }

    // Set light's colour based on the item's rarity from the itemSO
    private void SetLightColourBasedOnRarity()
    {
        switch (itemSO.itemRarity)
        {
            case ItemRarity.Common:
                _itemLight.color = GameManager.Instance.common;
                break;
            case ItemRarity.Uncommon:
                _itemLight.color = GameManager.Instance.uncommon;
                break;
            case ItemRarity.Rare:
                _itemLight.color = GameManager.Instance.rare;
                break;
            case ItemRarity.Epic:
                _itemLight.color = GameManager.Instance.epic;
                break;
            case ItemRarity.Legendary:
                _itemLight.color = GameManager.Instance.legendary;
                break;
            case ItemRarity.Mythic:
                _itemLight.color = GameManager.Instance.mythic;
                break;
            case ItemRarity.Otherworldly:
                _itemLight.color = GameManager.Instance.otherworldly;
                break;
            default:
                Debug.LogWarning($"Item rarity Not Set for {gameObject.name}!");
                StartCoroutine(GameManager.Instance.FlashRed(_itemLight));
                break;
        }
    }

    // to prevent the light from instantly turning off when the player gets too far-
    // fade the light's instensity based on how far the player is compared to the max allowed distance
    private float startingLightIntensity;
    private void AdjustLightIntensity()
    {
        float distanceToPlayer = Vector3.Distance(GameManager.Instance.player.transform.position, transform.position);

        // Check if the player is within active range of the light
        if (distanceToPlayer <= GameManager.Instance.settingsSO.lightRenderDistance)
        {
            // Calculate a normalized value (0 to 1) based on the player's distance
            float normalizedDistance = distanceToPlayer / GameManager.Instance.settingsSO.lightRenderDistance;

            // Interpolate the light's intensity from its maximum value to 0
            _itemLight.intensity = Mathf.Lerp(startingLightIntensity, 0, normalizedDistance);
        }
    }

    private void MergeItemStacks()
    {
        // when items of the same type are near each other (dont forget about rarity), merge them to save performance
        // Each item has a quantity vriables
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Item : MonoBehaviour
{
    public ItemSO itemSO;
    public float spriteDisableDistance = 48f, lightDisableDistance = 24f, physicsDisableDistance = 64f;

    private Collider itemCollider;
    private Rigidbody itemRB;
    private SpriteRenderer itemSR;
    private Light itemLight;

    public float bobbingHeight = 0.2f;
    public float bobbingSpeed = 2f, bobbingSpeedOffset = 0.5f;

    private void OnEnable()
    {
        // 1. Get components
        itemCollider = GetComponent<Collider>();
        itemRB = GetComponent<Rigidbody>();
        itemSR = GetComponentInChildren<SpriteRenderer>();
        itemLight = GetComponentInChildren<Light>();

        // 2. Set the variables
        originalY = itemSR.transform.localPosition.y;
        bobbingSpeed = Random.Range(bobbingSpeed - bobbingSpeedOffset, bobbingSpeed + bobbingSpeedOffset);
        itemRB.mass = itemSO.itemMass;
        if(itemRB.mass < 0.25f) itemRB.mass = 0.25f;

        // Since this is in the OnEnable function-
        // this makes sure the starting intensity doesn't get set again-
        // when the item leaves the player's range and comes back
        if (startingLightIntensity == 0) startingLightIntensity = itemLight.intensity;

        // Check if ScriptableObject is assigned
        if (itemSO == null)
        {
            Debug.LogWarning($"ScriptableObject is missing on the item {gameObject.name}!");
            itemSR.sprite = GameManager.Instance.missingSprite;
            StartCoroutine(FlashRed(itemLight));
            return;
        }

        // Set the sprite                 if true set itemSO's sprite | if false get fallback sprite
        itemSR.sprite = itemSO.itemSprite != null ? itemSO.itemSprite : GameManager.Instance.missingSprite;

        // If itemSO is assigned but has no sprite variable assigned... do:
        if (itemSO.itemSprite == null)
        {
            Debug.LogWarning($"Item sprite is missing in the ScriptableObject for the item: {gameObject.name}!");
            StartCoroutine(FlashRed(itemLight));
        }

        SetLightColourBasedOnRarity();
    }

    private void FixedUpdate()
    {
        DistanceCulling();
        if (spriteEnabled) { SpriteFacePlayer(); BobbingMotion();}
        if (lightEnabled) { AdjustLightIntensity();}
    }

    // if something fails on an item, use this function-
    // to make the object easier to locate in-game
    private IEnumerator FlashRed(Light light)
    {
        while (true)
        {
            light.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            light.color = Color.black;
            yield return new WaitForSeconds(0.2f);
        }
    }

    // Trun the sprite of the item to always face the camera
    private void SpriteFacePlayer()
    {
        Vector3 directionToCamera = GameManager.Instance.cameraPoint.transform.position - transform.position;

        directionToCamera = -directionToCamera;
        directionToCamera.y = 0;
        itemSR.transform.forward = directionToCamera;
    }

    // The item sprite bobs up and down slowly
    private float originalY;
    private void BobbingMotion()
    {
        // Calculate the new Y position
        float newY = originalY + bobbingHeight * Mathf.Sin(Time.time * bobbingSpeed);

        // Update the localPosition of the item
        itemSR.transform.localPosition = new Vector3(0, newY, 0);
    }

    // for performance reasons, make different components of an item get disabled-
    // once the player gets a specified distance away
    private bool spriteEnabled;
    private bool lightEnabled;
    private bool physicsEnabled;
    private void DistanceCulling()
    {
        float distanceToPlayer = Vector3.Distance(GameManager.Instance.player.transform.position, transform.position);

        spriteEnabled = distanceToPlayer <= spriteDisableDistance;
        lightEnabled = distanceToPlayer <= lightDisableDistance;
        physicsEnabled = distanceToPlayer <= physicsDisableDistance;

        if (itemSR != null) itemSR.gameObject.SetActive(spriteEnabled);
        if (itemLight != null) itemLight.gameObject.SetActive(lightEnabled);
        if (itemRB != null) itemRB.isKinematic = !physicsEnabled;
        if (itemCollider != null) itemCollider.enabled = physicsEnabled;
    }

    // Set light's colour based on the item's rarity from the itemSO
    private void SetLightColourBasedOnRarity()
    {
        switch (itemSO.itemRarity)
        {
            case ItemRarity.Common:
                itemLight.color = GameManager.Instance.common;
                break;
            case ItemRarity.Uncommon:
                itemLight.color = GameManager.Instance.uncommon;
                break;
            case ItemRarity.Rare:
                itemLight.color = GameManager.Instance.rare;
                break;
            case ItemRarity.Epic:
                itemLight.color = GameManager.Instance.epic;
                break;
            case ItemRarity.Legendary:
                itemLight.color = GameManager.Instance.legendary;
                break;
            case ItemRarity.Mythic:
                itemLight.color = GameManager.Instance.mythic;
                break;
            case ItemRarity.Otherworldly:
                itemLight.color = GameManager.Instance.otherworldly;
                break;
            default:
                Debug.LogWarning($"Item rarity Not Set for {gameObject.name}!");
                StartCoroutine(FlashRed(itemLight));
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
        if (distanceToPlayer <= lightDisableDistance)
        {
            // Calculate a normalized value (0 to 1) based on the player's distance
            float normalizedDistance = distanceToPlayer / lightDisableDistance;

            // Interpolate the light's intensity from its maximum value to 0
            itemLight.intensity = Mathf.Lerp(startingLightIntensity, 0, normalizedDistance);
        }
    }
}

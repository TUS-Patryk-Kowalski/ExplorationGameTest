using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Item : MonoBehaviour
{
    public ItemSO itemSO;
    public float disableDistance = 48f;

    private Collider itemCollider;
    private Rigidbody itemRB;
    private SpriteRenderer itemSR;
    private Light itemLight;

    public float bobbingHeight = 0.2f;
    public float bobbingSpeed = 2f, bobbingSpeedOffset = 0.5f;
    private float originalY;

    private void OnEnable()
    {
        // Ensure GameManager is available
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is not available!");
            return; // Exit early if GameManager is not available
        }

        // Get components
        itemCollider = GetComponent<Collider>();
        itemRB = GetComponent<Rigidbody>();
        itemSR = GetComponentInChildren<SpriteRenderer>();
        itemLight = GetComponentInChildren<Light>();

        // Set variables
        originalY = itemSR.transform.localPosition.y;
        bobbingSpeed = Random.Range(bobbingSpeed - bobbingSpeedOffset, bobbingSpeed + bobbingSpeedOffset);

        // Check for missing components
        if (itemSR == null)
        {
            Debug.LogWarning($"SpriteRenderer is missing on the item {gameObject.name}!");
            return;
        }
        if (itemLight == null)
        {
            Debug.LogWarning($"Light is missing on the item {gameObject.name}!");
            return;
        }

        // Check if ScriptableObject is assigned
        if (itemSO == null)
        {
            Debug.LogWarning($"ScriptableObject is missing on the item {gameObject.name}!");
            itemSR.sprite = GameManager.Instance.missingSprite;
            StartCoroutine(FlashRed(itemLight));
            return;
        }

        // Set the sprite
        itemSR.sprite = itemSO.itemSprite != null ? itemSO.itemSprite : GameManager.Instance.missingSprite;
        if (itemSO.itemSprite == null)
        {
            Debug.LogWarning($"Item sprite is missing in ScriptableObject for the item {gameObject.name}!");
            StartCoroutine(FlashRed(itemLight));
        }

        // Set light color based on item rarity
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

    private void FixedUpdate()
    {
        SpriteFacePlayer();
        BobbingMotion();
        DistanceCulling();
    }

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

    private void SpriteFacePlayer()
    {
        Vector3 directionToCamera = GameManager.Instance.cameraPoint.transform.position - transform.position;

        directionToCamera = -directionToCamera;
        transform.forward = directionToCamera;
    }

    private void BobbingMotion()
    {
        // Calculate the new Y position
        float newY = originalY + bobbingHeight * Mathf.Sin(Time.time * bobbingSpeed);

        // Update the localPosition of the item
        itemSR.transform.localPosition = new Vector3(0, newY, 0);
    }

    private void DistanceCulling()
    {
        float distanceToPlayer = Vector3.Distance(GameManager.Instance.player.transform.position, transform.position);
        bool shouldEnable = distanceToPlayer <= disableDistance;

        if (itemSR != null) itemSR.enabled = shouldEnable;
        if (itemLight != null) itemLight.enabled = shouldEnable;
        if (itemRB != null) itemRB.isKinematic = !shouldEnable;
        if (itemCollider != null) itemCollider.enabled = shouldEnable;
    }
}

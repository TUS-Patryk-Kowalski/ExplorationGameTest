using Common.Enums;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DungeonEntrance : MonoBehaviour
{
    public Rarity dungeonRarity;
    public bool playerInTrigger;

    public GameObject entrance;
    public ParticleSystem entranceParticles;
    public float fadeOutDuration = 2f;

    public Animator doorAnimator;
    public Coroutine doorParticleIEnum;

    public DungeonController dungeonController;
    public DungeonRenderer dungeonRenderer;

    private void OnEnable()
    {
        doorAnimator = GetComponent<Animator>();
        entranceParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        var mainModule = entranceParticles.main;
        mainModule.startColor = SetColourByDungeonLevel();

        dungeonController = GetComponentInChildren<DungeonController>();
        dungeonController.readyToGenerate = true; 
        dungeonController.enabled = false;

        dungeonRenderer = GetComponentInChildren<DungeonRenderer>();
        dungeonRenderer.DungeonRenderUpdate();
    }

    private void Update()
    {
        if(playerInTrigger && Keyboard.current.eKey.wasPressedThisFrame)
        {
            dungeonController.enabled = true;
            dungeonRenderer.DungeonRenderUpdate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInTrigger = true;
            entranceParticles.gameObject.SetActive(true);

            var mainModule = entranceParticles.main;
            Color reset = mainModule.startColor.color;
            reset.a = 1;
            mainModule.startColor = reset;
            entranceParticles.Play();
            doorAnimator.SetBool("PlayerInTrigger", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInTrigger = false;
            doorParticleIEnum = StartCoroutine(FadeParticles());
            doorAnimator.SetBool("PlayerInTrigger", false);
        }
    }

    private IEnumerator FadeParticles()
    {
        // Fade out door particles until opacity is 0
        // once the new particle opacity is 0, wait for however long the particles stay for (particle lifespan)
        // after the time is up, stop, then disable the particle system
        yield return null;
    }

    private Color SetColourByDungeonLevel()
    {
        switch (dungeonRarity)
        {
            case Rarity.Common:
                return GameManager.Instance.common;
            case Rarity.Uncommon:
                return GameManager.Instance.uncommon;
            case Rarity.Rare:
                return GameManager.Instance.rare;
            case Rarity.Epic:
                return GameManager.Instance.epic;
            case Rarity.Legendary:
                return GameManager.Instance.legendary;
            case Rarity.Mythic:
                return GameManager.Instance.mythic;
            case Rarity.Otherworldly:
                return GameManager.Instance.otherworldly;
            default:
                return Color.red;
        }
    }
}

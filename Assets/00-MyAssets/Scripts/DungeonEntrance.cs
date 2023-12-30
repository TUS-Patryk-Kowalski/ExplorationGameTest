using Common.Enums;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DungeonEntrance : MonoBehaviour
{
    public DungeonController dungeonController;
    public DungeonRenderer dungeonRenderer;
    public Rarity dungeonRarity;

    private bool playerInTrigger;
    private ParticleSystem entranceParticles;
    private Animator doorAnimator;
    private Coroutine doorParticleIEnum;

    private void OnEnable()
    {
        if(!doorAnimator) doorAnimator = GetComponent<Animator>();
        if(!entranceParticles) entranceParticles = GetComponentInChildren<ParticleSystem>();
        if (!dungeonController) dungeonController = GetComponentInChildren<DungeonController>();
        if (!dungeonRenderer) dungeonRenderer = GetComponentInChildren<DungeonRenderer>();
    }

    private void Start()
    {
        var mainModule = entranceParticles.main;
        mainModule.startColor = SetColourByDungeonLevel();
        dungeonController.readyToGenerate = true; 
        dungeonController.enabled = false;
        dungeonRenderer.DungeonRenderUpdate();
    }

    private void Update()
    {
        if(playerInTrigger && Keyboard.current.eKey.wasPressedThisFrame && dungeonController.generatedDungeon)
        {
            dungeonController.enabled = true;
            dungeonRenderer.DungeonRenderUpdate();
            GameManager.Instance.MovePlayer(dungeonController.gameObject.transform, new Vector3(0, 2, 0));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (dungeonController.readyToGenerate && !dungeonController.generatedDungeon)
            {
                dungeonController.GenerateDungeon();
            }

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
        // Stop emitting new particles
        entranceParticles.Stop();

        // Wait for existing particles to fade out and die
        while (entranceParticles.IsAlive(true))
        {
            yield return null;
        }

        entranceParticles.gameObject.SetActive(false);
        StopCoroutine(doorParticleIEnum);
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

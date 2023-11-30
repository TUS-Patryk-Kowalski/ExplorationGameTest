using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject cameraPoint;
    public GameObject player;

    public Sprite missingSprite;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        player = GameObject.FindWithTag("Player");
        cameraPoint = GameObject.Find("PlayerFollowCamera");
    }

    public Color common, uncommon, rare, epic, legendary, mythic, otherworldly;
}

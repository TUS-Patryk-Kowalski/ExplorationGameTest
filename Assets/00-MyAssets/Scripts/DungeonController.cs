using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : MonoBehaviour
{
    public bool readyToGenerate;
    private bool generatedDungeon;

    private void OnEnable()
    {
        if (!generatedDungeon && readyToGenerate)
        {
            GenerateDungeon();
            GameManager.Instance.MovePlayer(transform, new Vector3(0,2,0));
        }
    }

    private void GenerateDungeon()
    {
        // Code for generating the dungeon
        generatedDungeon = true;
    }
}

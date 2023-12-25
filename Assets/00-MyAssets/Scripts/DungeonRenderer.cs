using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRenderer : MonoBehaviour
{
    private DungeonController dungeonController;

    public void Awake()
    {
        dungeonController = GetComponent<DungeonController>();
    }

    public void DungeonRenderUpdate()
    {
        if (dungeonController && dungeonController.enabled)
        {
            foreach(Transform child in dungeonController.gameObject.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform child in dungeonController.gameObject.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}

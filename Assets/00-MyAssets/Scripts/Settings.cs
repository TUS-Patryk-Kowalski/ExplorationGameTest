using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public int framerate;

    void Start()
    {
        Application.targetFrameRate = framerate;
    }
}

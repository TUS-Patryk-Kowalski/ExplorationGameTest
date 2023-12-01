using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSettingsObject", menuName = "Inventory/Items/Settings")]
public class ItemSettingsSO : ScriptableObject
{
    public float spriteRenderDistance;
    public float lightRenderDistance;
    public float physicsActiveDistance;
}

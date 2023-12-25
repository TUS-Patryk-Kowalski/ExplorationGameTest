using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            Item item = other.gameObject.GetComponent<Item>();
            if (!item.itemSO) 
            {
                Debug.LogWarning("You tried to pick up an incomplete item, it got discarded instead!");
                GameObject.Destroy(item.gameObject);
            } 
            GameManager.Instance.inventoryManager.AddItemToInventory(item);
        }
    }
}

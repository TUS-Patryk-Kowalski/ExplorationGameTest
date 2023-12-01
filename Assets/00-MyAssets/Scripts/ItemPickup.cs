using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            Debug.Log("Hit an Item");
            Item item = other.gameObject.GetComponent<Item>();
            if (item != null) { Debug.Log("Got the Item component"); }
            GameManager.Instance.inventoryManager.AddItemToInventory(item);
        }
    }
}

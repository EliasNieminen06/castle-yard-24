using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    [SerializeField] List<InventorySlot> inventorySlots = new List<InventorySlot>();

    public void Init(int maxItems)
    {
        for (int i = inventorySlots.Count - 1; i >= maxItems; i--)
        {
            inventorySlots[i].gameObject.SetActive(false);
            inventorySlots.RemoveAt(i);
        }
    }

    public void AddItemToInventory(UpgradeItem item)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.item == null)
            {
                slot.SetItem(item);
                return;
            }
        }
    }

    public void UpdateItem(UpgradeItem item, Hero hero)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.item == item)
            {
                slot.UpdateSlot(hero);
                return;
            }
        }
    }
}

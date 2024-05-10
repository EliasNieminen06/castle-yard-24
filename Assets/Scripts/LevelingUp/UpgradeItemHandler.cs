using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeItemHandler
{
    readonly int maxItems;
    public List<UpgradeItem> itemsInInventory { get; private set; }
    private List<UpgradeItem> itemDrawPool;

    public bool ItemsMaxed (Hero hero)
    {
        if (itemsInInventory.Count < maxItems) return false;

        foreach (var item in itemsInInventory)
        {
            if (hero.Stats.Mediator.GetModifierStacks(item.modifierConfig.name) < item.modifierConfig.maxStacks)
            {
                return false;
            }
        }

        return true;
    }

    public void AddItemToInventory(UpgradeItem item)
    {
        itemsInInventory.Add(item);
        //Debug.Log($"Added {item.modifierConfig.name} to inventory, Items in inventoy: {itemInventory.Count}, Inventory capacity: {maxItems}");

        if (itemsInInventory.Count == maxItems)
        {
           //Debug.Log("Inventory Maxed Removing other items from draw pool");
           //foreach (var upgradeItem in itemDrawPool.ToArray())
           //{
           //    if (itemInventory.Contains(upgradeItem)) continue;
           //
           //    RemoveItemFromDrawPool(upgradeItem);
           //    Debug.Log("Removed " + upgradeItem + " From DrawPool");
           //}

            itemDrawPool.RemoveAll(upgradeItem => !itemsInInventory.Contains(upgradeItem));
        }
    }

    public void RemoveItemFromDrawPool(UpgradeItem item)
    {
        if (itemDrawPool.Contains(item))
        {
            itemDrawPool.Remove(item);
            //Debug.Log("Removed " + item + " From DrawPool");
        }
    }
    public void AddItemToDrawPool(UpgradeItem item)
    {
        if (itemsInInventory.Count >= maxItems) return;
        if (!itemDrawPool.Contains(item)) itemDrawPool.Add(item);
    }

    public void AddItemsToList(List<UpgradeItem> listToAddTo, int amountToAdd)
    {
        for (int i = 0; i < amountToAdd; i++)
        {
            if (itemDrawPool.Count == i)
            {
                Debug.LogWarning("Not Enough items in drawpool for all options");
                break;
            }
            listToAddTo.Add(GetRandomItem(listToAddTo));
        }
    }

    private UpgradeItem GetRandomItem(List<UpgradeItem> listToAddTo)
    {
        Dictionary<UpgradeItem, ValueTuple<int, int>> itemDictionary = new Dictionary<UpgradeItem, ValueTuple<int, int>>();
        int totalWeight = 0;

        foreach (var upgradeItem in itemDrawPool)
        {
            ValueTuple<int, int> tuple = (totalWeight, 0);
            totalWeight += upgradeItem.weight;
            tuple.Item2 = totalWeight - 1;
            itemDictionary.Add(upgradeItem, tuple);
        }

        int randomWeight = UnityEngine.Random.Range(1, totalWeight);
        //Debug.Log("Trying to get upgrade with a weight of: " + randomWeight);
        UpgradeItem item = GetUpgradeWithWeight(randomWeight);

        while (listToAddTo.Contains(item))
        {
            randomWeight = UnityEngine.Random.Range(0, totalWeight);
            //Debug.Log("Trying to get upgrade with a weight of: " + randomWeight);
            item = GetUpgradeWithWeight(randomWeight);
            if (listToAddTo.Count == itemDrawPool.Count) break;
        }

        //Debug.Log("Got Upgrade: " + item);
        return item;

        UpgradeItem GetUpgradeWithWeight(int weight)
        {
            foreach (var upgradeItem in itemDrawPool)
            {
                ValueTuple<int, int> rangeTuple = (0, 0);
                if (itemDictionary.TryGetValue(upgradeItem, out rangeTuple) == false)
                {
                    throw new IndexOutOfRangeException("Upgrade not in Dictionary");
                }

                if (rangeTuple.Item1 <= weight && rangeTuple.Item2 >= weight)
                {
                    //Debug.Log(rangeTuple + " " + weight);
                    return upgradeItem;
                }
            }

            throw new ArgumentOutOfRangeException("Did not find an upgrade with given weight");
        }
    }

    public UpgradeItemHandler(List<UpgradeItem> upgradeItems, int maxItems)
    {
        this.maxItems = maxItems;

        itemDrawPool = new List<UpgradeItem>();
        itemsInInventory = new List<UpgradeItem>();

        foreach (var item in upgradeItems)
        {
            AddItemToDrawPool(item);
        }
    }
}

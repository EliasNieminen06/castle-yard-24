using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessUpgradeHandler
{
    readonly List<StatUpgrade> statUpgrades;

    [Header("Rarity Colors")]
    readonly Color commonColor;
    readonly Color rareColor;
    readonly Color epicColor;
    readonly Color legendaryColor;

    private List<StatUpgrade> currentUpgrades;

    public void AddUpgradesToList(List<StatUpgrade> listToAddTo, int amountToAdd)
    {
        for (int i = 0; i < amountToAdd; i++)
        {
            if (statUpgrades.Count == i)
            {
                Debug.LogWarning("Not Enough Upgrades Available for all options");
                break;
            }
            listToAddTo.Add(GetRandomUpgrade(listToAddTo));
        }
    }

    private StatUpgrade GetRandomUpgrade(List<StatUpgrade> currentUpgrades)
    {
        Dictionary<StatUpgrade, ValueTuple<int, int>> upgradeDictionary = new Dictionary<StatUpgrade, ValueTuple<int, int>>();
        int totalWeight = 0;

        foreach (var upgrade in statUpgrades)
        {
            ValueTuple<int, int> tuple = (totalWeight, 0);
            totalWeight += upgrade.weight;
            tuple.Item2 = totalWeight - 1;
            upgradeDictionary.Add(upgrade, tuple);
        }

        int randomWeight = UnityEngine.Random.Range(1, totalWeight);
        //Debug.Log("Trying to get upgrade with a weight of: " + randomWeight);
        StatUpgrade currentUpgrade = GetUpgradeWithWeight(randomWeight);

        while (currentUpgrades.Contains(currentUpgrade))
        {
            randomWeight = UnityEngine.Random.Range(0, totalWeight);
            //Debug.Log("Trying to get upgrade with a weight of: " + randomWeight);
            currentUpgrade = GetUpgradeWithWeight(randomWeight);
        }

        //Debug.Log("Got Upgrade: " + currentUpgrade);
        return currentUpgrade;

        StatUpgrade GetUpgradeWithWeight(int weight)
        {
            foreach (var upgrade in statUpgrades)
            {
                ValueTuple<int, int> rangeTuple = (0, 0);
                if (upgradeDictionary.TryGetValue(upgrade, out rangeTuple) == false)
                {
                    throw new IndexOutOfRangeException("Upgrade not in Dictionary");
                }

                if (rangeTuple.Item1 <= weight && rangeTuple.Item2 >= weight)
                {
                    //Debug.Log(rangeTuple + " " + weight);
                    return upgrade;
                }
            }

            throw new ArgumentOutOfRangeException("Did not find an upgrade with given weight");
        }
    }

    /// <summary>
    /// Rolls rarity for a stat upgrade, Returns a tuple with modifier config and color for rarity
    /// </summary>
    public (StatModifierConfig config, Color color) RollRarity(StatUpgrade upgrade)
    {
        int common = upgrade.commonWeight;
        int rare = common + upgrade.rareWeight;
        int epic = rare + upgrade.epicWeight;
        int legendary = epic + upgrade.legendaryWeight;

        int randomWeight = UnityEngine.Random.Range(1, legendary + 1);

        StatModifierConfig config;
        Color rarityColor;

        if (randomWeight <= common)
        {
            config = upgrade.common;
            rarityColor = commonColor;
        }
        else if (randomWeight <= rare)
        {
            config = upgrade.rare;
            rarityColor = rareColor;
        }
        else if (randomWeight <= epic)
        {
            config = upgrade.epic;
            rarityColor = epicColor;
        }
        else if (randomWeight <= legendary)
        {
            config = upgrade.legendary;
            rarityColor = legendaryColor;
        }
        else throw new System.ArgumentOutOfRangeException();

        return (config, rarityColor);
    }

    public EndlessUpgradeHandler(List<StatUpgrade> statUpgrades, Color commonColor,Color rareColor, Color epicColor, Color legendaryColor)
    {
        this.statUpgrades = statUpgrades;
        this.commonColor = commonColor;
        this.rareColor = rareColor;
        this.epicColor = epicColor;
        this.legendaryColor = legendaryColor;
    }
}

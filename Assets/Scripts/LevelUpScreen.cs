using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class LevelUpScreen : MonoBehaviour
{
    private static LevelUpScreen instance;

    //[SerializeField] private List<BaseStatModifier> statModifiers;
    [SerializeField] private List<StatUpgrade> statUpgrades;
    [SerializeField] private List<StatUpgradeOption> upgradeOptions;

    [Header("Rarity Colors")]
    [SerializeField] private Color commonColor;
    [SerializeField] private Color rareColor;
    [SerializeField] private Color epicColor;
    [SerializeField] private Color legendaryColor;

    private event System.Action OnHide = delegate { };

    private List<StatUpgrade> currentUpgrades;

    private void Awake()
    {
        instance = this;
        currentUpgrades = new List<StatUpgrade>();
        Hide();
    }

    private void Show(Hero hero, System.Action OnHide)
    {
        gameObject.SetActive(true);

        if (statUpgrades.Count == 0)
        {
            Debug.LogWarning("0 UpgradeOptions in Levelupscreen... Hiding");
            Hide();
            return;
        }

        SetUpgrades(hero);
        this.OnHide = OnHide;

        Time.timeScale = 0f;
    }

    private void Hide()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        OnHide.Invoke();
    }

    private void SetUpgrades(Hero hero)
    {
        currentUpgrades.Clear();
        AddUpgradesToList(currentUpgrades);

        for (int i = 0; i < upgradeOptions.Count; i++)
        {
            if (i == currentUpgrades.Count) break;

            var configAndColor = GetConfigAndColor(currentUpgrades[i]);
            upgradeOptions[i].SetUpgrade(hero, configAndColor.config, configAndColor.color);
        }
    }

    private void AddUpgradesToList(List<StatUpgrade> listToAddTo)
    {
        for (int i = 0; i < upgradeOptions.Count; i++)
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

    private (StatModifierConfig config, Color color) GetConfigAndColor(StatUpgrade upgrade)
    {
        int common = upgrade.commonWeight;
        int rare = common + upgrade.rareWeight;
        int epic = rare + upgrade.epicWeight;
        int legendary = epic + upgrade.legendaryWeight;

        int randomWeight = UnityEngine.Random.Range(1, legendary + 1);

        StatModifierConfig config;
        Color rarityColor;

        if (randomWeight <= common) {
            config = upgrade.common;
            rarityColor = commonColor;
        }
        else if (randomWeight <= rare) {
            config = upgrade.rare;
            rarityColor = rareColor;
        }
        else if (randomWeight <= epic) {
            config = upgrade.epic;
            rarityColor = epicColor;
        }
        else if (randomWeight <= legendary) {
            config = upgrade.legendary;
            rarityColor = legendaryColor;
        }
        else throw new System.ArgumentOutOfRangeException();

        return (config, rarityColor);
    }

    public static void Show_Static(Hero hero, System.Action onHide)
    {
        instance.Show(hero, onHide);
    }

    public static void Hide_Static()
    {
        instance.Hide();
    }
}

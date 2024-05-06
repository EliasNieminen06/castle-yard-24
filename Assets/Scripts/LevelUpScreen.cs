using UnityEngine;
using System.Collections.Generic;

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
        int randomIndex = Random.Range(0, statUpgrades.Count);

        while (currentUpgrades.Contains(statUpgrades[randomIndex]))
        {
            randomIndex = Random.Range(0, statUpgrades.Count);
        }

        return statUpgrades[randomIndex];
    }

    private (StatModifierConfig config, Color color) GetConfigAndColor(StatUpgrade upgrade)
    {
        int common = upgrade.commonWeight;
        int rare = common + upgrade.rareWeight;
        int epic = rare + upgrade.epicWeight;
        int legendary = epic + upgrade.legendaryWeight;

        int randomWeight = Random.Range(1, legendary + 1);

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

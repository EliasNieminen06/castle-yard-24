using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class LevelUpScreen : MonoBehaviour
{
    private static LevelUpScreen instance;

    [SerializeField] ItemInventory inventory;

    //[SerializeField] private List<BaseStatModifier> statModifiers;
    [SerializeField] private List<UpgradeItem> upgradeItems;
    [SerializeField] private List<StatUpgrade> endlessUpgrades;

    [SerializeField] private List<StatUpgradeOption> upgradeOptions;

    private EndlessUpgradeHandler endlessUpgradeHandler;
    private UpgradeItemHandler itemHandler;

    [Header("Rarity Colors")]
    [SerializeField] private Color commonColor;
    [SerializeField] private Color rareColor;
    [SerializeField] private Color epicColor;
    [SerializeField] private Color legendaryColor;

    private event System.Action OnHide = delegate { };

    private List<StatUpgrade> currentUpgrades;
    private List<UpgradeItem> currentItems;

    private const float TIME_BEFORE_CAN_CHOOSE_UPGRADE = 0.3f;

    private void Awake()
    {
        instance = this;
        currentUpgrades = new List<StatUpgrade>();
        currentItems = new List<UpgradeItem>();
        endlessUpgradeHandler = new EndlessUpgradeHandler(endlessUpgrades, commonColor, rareColor, epicColor, legendaryColor);
        itemHandler = new UpgradeItemHandler(upgradeItems, 6);
        inventory.Init(6);
        Hide();
    }

    private void Show(Hero hero, System.Action OnHide)
    {
        gameObject.SetActive(true);

        float ableToApplyUpgradeTime = Time.realtimeSinceStartup + TIME_BEFORE_CAN_CHOOSE_UPGRADE;

        if (endlessUpgrades.Count == 0)
        {
            Hide();
            throw new NullReferenceException("0 StatUpgrades Available");
        }

        //upgradeOptions.ForEach((upgradeOption) => upgradeOption.gameObject.SetActive(true));

        if (!itemHandler.ItemsMaxed(hero)) SetItems(hero, ableToApplyUpgradeTime);
        else SetUpgrades(hero, ableToApplyUpgradeTime);

        this.OnHide = OnHide;

        Time.timeScale = 0f;
    }

    private void Hide()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        OnHide.Invoke();
    }

    private void SetUpgrades(Hero hero, float ableToApplyUpgradeTime)
    {
        currentUpgrades.Clear();
        endlessUpgradeHandler.AddUpgradesToList(currentUpgrades, upgradeOptions.Count);

        for (int i = 0; i < upgradeOptions.Count; i++)
        {
            if (i == currentUpgrades.Count) break;

            var rarity = endlessUpgradeHandler.RollRarity(currentUpgrades[i]);
            upgradeOptions[i].SetUpgrade(hero, rarity.config, rarity.color, true, ableToApplyUpgradeTime, (int stacks) => {
                Debug.Log(rarity.config.name + " " + stacks);
            });
        }
    }

    private void SetItems(Hero hero, float ableToApplyUpgradeTime)
    {
        currentItems.Clear();
        itemHandler.AddItemsToList(currentItems, upgradeOptions.Count);

        for (int i = 0; i < upgradeOptions.Count; i++)
        {
            if (i == currentItems.Count)
            {
                upgradeOptions[i].gameObject.SetActive(false);
                break;
            }

            UpgradeItem item = currentItems[i];

            upgradeOptions[i].SetUpgrade(hero, item.modifierConfig, item.color, false, ableToApplyUpgradeTime, (int stacks) => {
                if (stacks >= item.modifierConfig.maxStacks)
                {
                    itemHandler.RemoveItemFromDrawPool(item);
                }

                if (!itemHandler.itemsInInventory.Contains(item))
                {
                    itemHandler.AddItemToInventory(item);
                    inventory.AddItemToInventory(item);
                }

                inventory.UpdateItem(item, hero);

                Debug.Log(item.modifierConfig.name + " " + stacks);
            });
        }
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

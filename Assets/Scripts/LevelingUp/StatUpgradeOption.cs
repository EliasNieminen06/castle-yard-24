using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatUpgradeOption : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI upgradeNameTMP;
    [SerializeField] private TextMeshProUGUI upgradeLevelTMP;
    [SerializeField] private TextMeshProUGUI upgradeDescriptionTMP;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Image icon;

    private float ableToApplyTime;

    public void SetUpgrade(Hero hero, StatModifierConfig modifierConfig, Color upgradeColor, bool showStatInsteadOfName, float ableToApplyTime, System.Action<int> OnApply)
    {
        gameObject.SetActive(true);

        this.ableToApplyTime = ableToApplyTime;
        //icon.color = backgroundColor;

        if (showStatInsteadOfName) upgradeNameTMP.text = modifierConfig.type.ToString();
        else upgradeNameTMP.text = modifierConfig.name;

        upgradeNameTMP.color = upgradeColor;

        if (showStatInsteadOfName) upgradeLevelTMP.text = $"Lv: {hero.Stats.Mediator.GetModifierStacks(modifierConfig.name) + 1}";
        else upgradeLevelTMP.text = $"Lv: {hero.Stats.Mediator.GetModifierStacks(modifierConfig.name) + 1} / {modifierConfig.maxStacks}";
        upgradeDescriptionTMP.text = modifierConfig.description;

        icon.sprite = modifierConfig.icon;

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() =>
        {
            ApplyUpgrade(hero, modifierConfig, OnApply);
        });
    }

    public void ApplyUpgrade(Hero hero, StatModifierConfig modifierConfig, System.Action<int> OnApply)
    {
        if (Time.realtimeSinceStartup < ableToApplyTime) return;

        StatsMediator mediator = hero.Stats.Mediator;
        if (mediator.TryModifyModifier(modifierConfig.name))
        {
            OnApply.Invoke(mediator.GetModifierStacks(modifierConfig.name));
            LevelUpScreen.Hide_Static();
            return;
        }

        StatModifier modifier;
        if (modifierConfig.stackable) modifier = new StackableStatModifier(modifierConfig);
        else modifier = new BasicStatModifier(modifierConfig);

        mediator.AddModifier(modifier);
        OnApply.Invoke(mediator.GetModifierStacks(modifierConfig.name));

        LevelUpScreen.Hide_Static();
    }
}

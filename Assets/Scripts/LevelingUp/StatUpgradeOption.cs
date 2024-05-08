using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatUpgradeOption : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI upgradeNameText;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Image background;

    public void SetUpgrade(Hero hero, StatModifierConfig modifierConfig, Color backgroundColor, System.Action<int> OnApply)
    {
        background.color = backgroundColor;
        upgradeNameText.text = $"{modifierConfig.type} + {modifierConfig.value}";

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() =>
        {
            ApplyUpgrade(hero, modifierConfig, OnApply);
            LevelUpScreen.Hide_Static();
        });
    }

    public void ApplyUpgrade(Hero hero, StatModifierConfig modifierConfig, System.Action<int> OnApply)
    {
        StatsMediator mediator = hero.Stats.Mediator;
        if (mediator.TryModifyModifier(modifierConfig.name))
        {
            OnApply.Invoke(mediator.GetModifierStacks(modifierConfig.name));
            return;
        }

        StatModifier modifier;
        if (modifierConfig.stackable) modifier = new StackableStatModifier(modifierConfig);
        else modifier = new BasicStatModifier(modifierConfig);

        mediator.AddModifier(modifier);
        OnApply.Invoke(mediator.GetModifierStacks(modifierConfig.name));
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatUpgrade : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI upgradeNameText;
    [SerializeField] private Button upgradeButton;

    public void SetUpgrade(Hero hero, BaseStatModifier statModifier)
    {
        upgradeNameText.text = statModifier.name;

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() =>
        {
            ApplyUpgrade(hero, statModifier);
            LevelUpScreen.Hide_Static();
        });
    }

    public void ApplyUpgrade(Hero hero, BaseStatModifier baseModifier)
    {
        StatsMediator mediator = hero.Stats.Mediator;
        if (mediator.TryModifyModifier(baseModifier.name)) return;

        StatModifier modifier;
        if (baseModifier.stackable) modifier = new StackableStatModifier(baseModifier);
        else modifier = new BasicStatModifier(baseModifier);

        mediator.AddModifier(modifier);
    }
}

using UnityEngine;

public class StatModifierPickup : Pickup
{
    [SerializeField] StatModifierPickupBase baseModifier;

    protected override void ApplyPickupEffect(Hero hero)
    {
        StatsMediator mediator = hero.Stats.Mediator;
        if (mediator.TryModifyModifier(baseModifier.name)) return;

        StatModifier modifier;
        if (baseModifier.config.stackable) modifier = new StackableStatModifier(baseModifier.config);
        else modifier = new BasicStatModifier(baseModifier.config);

        mediator.AddModifier(modifier);
    }
}

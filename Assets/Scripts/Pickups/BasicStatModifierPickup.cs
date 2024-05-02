using UnityEngine;

public class StatModifierPickup : Pickup
{
    [SerializeField] BaseStatModifier baseModifier;

    protected override void ApplyPickupEffect(Hero hero)
    {
        StatsMediator mediator = hero.Stats.Mediator;
        if (mediator.TryModifyModifier(baseModifier.name)) return;

        StatModifier modifier;
        if (baseModifier.stackable) modifier = new StackableStatModifier(baseModifier);
        else modifier = new BasicStatModifier(baseModifier);

        mediator.AddModifier(modifier);
    }
}

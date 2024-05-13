using UnityEngine;

public class StatModifierPickup : Pickup
{
    [SerializeField] StatModifierPickupBase baseModifier;

    protected override void ApplyPickupEffect(Hero hero)
    {
        StatsMediator mediator = hero.Stats.Mediator;
        if (baseModifier.maxStacks != 0 && mediator.GetModifierStacks(baseModifier.name) == baseModifier.maxStacks && !baseModifier.refreshable && !baseModifier.timeStackable) return;
        if (mediator.TryModifyModifier(baseModifier.name))
        {
            float timeToAdd = 0f;
            if (baseModifier.timeStackable) timeToAdd = baseModifier.duration;
            StatPickupManager.ModifyPickup_Static(baseModifier, baseModifier.refreshable, timeToAdd);
            return;
        }

        StatModifier modifier;
        if (baseModifier.config.stackable) modifier = new StackableStatModifier(baseModifier.config);
        else modifier = new BasicStatModifier(baseModifier.config);

        mediator.AddModifier(modifier);
        StatPickupManager.AddPickup_Static(baseModifier);
    }
}

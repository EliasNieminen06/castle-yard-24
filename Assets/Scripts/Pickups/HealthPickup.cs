using UnityEngine;

public class HealthPickup : Pickup
{
    [SerializeField] float value = 10;

    protected override void ApplyPickupEffect(Hero hero)
    {
        if (value < 0) hero.TakeDamage(-value);
        else hero.Heal(hero.Stats.MaxHp * value / 100);
    }
}

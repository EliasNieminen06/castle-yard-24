using UnityEngine;

public class ExpPickup : Pickup
{
    [SerializeField] int value = 10;

    protected override void ApplyPickupEffect(Hero hero)
    {
        hero.AddExperience(value);
    }
}

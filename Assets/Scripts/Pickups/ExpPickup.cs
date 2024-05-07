using UnityEngine;

public class ExpPickup : Pickup
{
    [field: SerializeField] public int value { get; private set; } = 10;


    protected override void ApplyPickupEffect(Hero hero)
    {
        hero.AddExperience(value);
        ExpPickupManager.RemoveExpPickup(this);
    }

    public override void Init()
    {
        ExpPickupManager.AddExpPickup(this);
        base.Init();
    }

    public void Absorb(ExpPickup expPickup)
    {
        value += expPickup.value;
        ExpPickupManager.RemoveExpPickup(expPickup);
        Destroy(expPickup.gameObject);
    }

    public void SetValue(int value)
    {
        this.value = value;
    }
}

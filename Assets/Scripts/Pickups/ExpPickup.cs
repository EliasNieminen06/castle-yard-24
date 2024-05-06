using UnityEngine;

public class ExpPickup : Pickup
{
    [field: SerializeField] public int value { get; private set; } = 10;

    private bool Initialized = false;

    protected override void ApplyPickupEffect(Hero hero)
    {
        hero.AddExperience(value);
        ExpPickupManager.RemoveExpPickup(this);
    }

    private void Awake()
    {
        if (Initialized) return;
        ExpPickupManager.AddExpPickup(this);
    }
    public void Init(int value)
    {
        Initialized = true;
        ExpPickupManager.AddExpPickup(this);
        this.value = value;
    }

    public void Absorb(ExpPickup expPickup)
    {
        value += expPickup.value;
        ExpPickupManager.RemoveExpPickup(expPickup);
        Destroy(expPickup.gameObject);
    }
}

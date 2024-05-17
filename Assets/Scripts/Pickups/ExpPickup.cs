using UnityEngine;

public class ExpPickup : Pickup
{
    [field: SerializeField] public int value { get; private set; } = 10;

    [Header("Sizes")]
    [SerializeField] private float tinySize = 0.15f;
    [SerializeField] private float smallSize = 0.25f;
    [SerializeField] private float mediumSize = 0.4f;
    [SerializeField] private float largeSize = 0.6f;
    [SerializeField] private float hugeSize = 0.8f;

    [Header("Breakpoints")]
    [SerializeField] private float tiny = 2f;
    [SerializeField] private float small = 5f;
    [SerializeField] private float medium = 10f;
    [SerializeField] private float large = 20f;
    [SerializeField] private float huge = 50f;

    protected override void ApplyPickupEffect(Hero hero)
    {
        hero.AddExperience(value);
        ExpPickupManager.RemoveExpPickup(this);
    }

    public override void Init()
    {
        ExpPickupManager.AddExpPickup(this);
        SetSizeBasedOnValue(value);
        base.Init();
    }

    public void Absorb(ExpPickup expPickup)
    {
        int newValue = value + expPickup.value;
        SetValue(newValue);
        ExpPickupManager.RemoveExpPickup(expPickup);
        Destroy(expPickup.gameObject);
    }

    public void SetValue(int value)
    {
        this.value = value;
        SetSizeBasedOnValue(value);
    }

    private void SetSizeBasedOnValue(int value)
    {
        if (value >= huge)
        {
            transform.localScale = Vector3.one * hugeSize;
        }
        else if (value >= large)
        {
            transform.localScale = Vector3.one * largeSize;
        }
        else if (value >= medium)
        {
            transform.localScale = Vector3.one * mediumSize;
        }
        else if (value >= small)
        {
            transform.localScale = Vector3.one * smallSize;
        }
        else
        {
            transform.localScale = Vector3.one * tinySize;
        }
    }
}

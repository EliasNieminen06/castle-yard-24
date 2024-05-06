using System;
using UnityEngine;

public class Health
{
    readonly Stats stats;

    public Action OnHealthChanged = delegate { };

    public float currentHp { get; private set; }
    public float currentTemporaryHp { get; private set; }
    public float temporaryHpDrainPerSecond { get; private set; }

    public Health(BaseStats baseStats, Stats stats)
    {
        this.stats = stats;

        SetHpToMax();
        //SetTemporaryHpToMax();
    }

    public void UpdateHp()
    {
        if (currentHp > stats.MaxHp) SetHpToMax();
    }

    public void AddHp(float amount)
    {
        if (currentHp >= stats.MaxHp) return;

        float hpAfterAdding = currentHp + amount;
        if (hpAfterAdding > stats.MaxHp)
        {    
            SetHpToMax();
            return;
        }
        
        SetCurrentHp(hpAfterAdding);

        //float hpAfterAdding = currentHp + amount;
        //if (hpAfterAdding > maxHp)
        //{
        //    if (overflow)
        //    {
        //        float tempHptoAdd = currentHp - maxHp + amount;
        //        AddTemporaryHp(tempHptoAdd);
        //    }
        //
        //    SetHpToMax();
        //    return;
        //}
        //
        //SetCurrentHp(hpAfterAdding);
    }
    public void ReduceHp(float amount)
    {
        if (currentTemporaryHp > 0)
        {
            // take temp hp before normal hp
        }
        SetCurrentHp(currentHp - amount);
    }
    public void SetHpToMax()
    {
        SetCurrentHp(stats.MaxHp);
    }
    public void SetCurrentHp(float amount)
    {
        currentHp = amount;
        OnHealthChanged.Invoke();
    }

    //public void SetMaxTemporaryHp(float amount)
    //{
    //    maxTemporaryHp = amount;
    //}
    //public void AddTemporaryHp(float amount)
    //{
    //    float temporaryHpAfterAdding = currentTemporaryHp + amount;
    //
    //    if (temporaryHpAfterAdding > maxTemporaryHp) SetTemporaryHpToMax();
    //    else SetCurrentTemporaryHp(temporaryHpAfterAdding);
    //}
    //public void SetTemporaryHpToMax()
    //{
    //    SetCurrentTemporaryHp(maxTemporaryHp);
    //}
    //public void SetCurrentTemporaryHp(float amount)
    //{
    //    currentTemporaryHp = amount;
    //}

    public override string ToString() => $"Hp {currentHp}/{stats.MaxHp}";
}

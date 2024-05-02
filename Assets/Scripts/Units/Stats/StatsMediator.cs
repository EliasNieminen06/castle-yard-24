using System;
using System.Collections.Generic;

public class StatsMediator
{
    readonly List<StatModifier> modifiers = new List<StatModifier>();

    public event Action OnModifiersChanged = delegate { };
    public event EventHandler<Query> Queries;
    public void PerformQuery(object sender, Query query) => Queries?.Invoke(sender, query);

    public void AddModifier(StatModifier modifier)
    {
        modifiers.Add(modifier);
        Queries += modifier.Handle;
        ArrangeModifiers();
        ModifiersChanged();

        modifier.OnDispose += _ =>
        {
            modifiers.Remove(modifier);
            Queries -= modifier.Handle;
            ModifiersChanged();
        };
    }

    /// <summary>
    /// Returns true if a modifier with given name was modified
    /// </summary>
    public bool TryModifyModifier(string name)
    {
        if (name == null) return false;

        StatModifier modifier = null;

        foreach (var mod in modifiers)
        {
            if (mod.name == name)
            {
                modifier = mod;
                break;
            }
        }

        if (modifier == null) return false;

        if (modifier.Modify())
        {
            ModifiersChanged();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ModifiersChanged()
    {
        OnModifiersChanged.Invoke();
    }

    private void ArrangeModifiers()
    {
        // Moves all multiplier modifiers to be calculated last
        foreach (var modifier in modifiers)
        {
            if (modifier.operatorType == StatModifier.OperatorType.Multiply)
            {
                Queries -= modifier.Handle;
                Queries += modifier.Handle;
            }
        }
    }

    public void Update()
    {
        // Update all modifiers
        foreach (var modifier in modifiers)
        {
            modifier.Update();
        }

        // Dispose modifier if marked for removal
        for (int i = 0; i < modifiers.Count; i++)
        {
            var modifier = modifiers[i];

            if (modifier.MarkedForRemoval)
            {
                modifier.Dispose();
            }
        } 
    }
}

public class Query
{
    public readonly StatType StatType;
    public float Value;

    public Query(StatType statType, float value)
    {
        StatType = statType;
        Value = value;
    }
}



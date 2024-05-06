using System;
using System.Collections.Generic;

public class StatsMediator
{
    readonly List<StatModifier> modifiers = new List<StatModifier>();

    public event EventHandler<ModifierChangedArgs> OnModifierChanged;
    public event EventHandler<Query> Queries;
    public void PerformQuery(object sender, Query query) => Queries?.Invoke(sender, query);

    public void AddModifier(StatModifier modifier)
    {
        modifiers.Add(modifier);
        Queries += modifier.Handle;
        ArrangeModifiers();
        ModifiersChanged(this, new ModifierChangedArgs(modifier, ModifierChangedArgs.Change.Added));

        modifier.OnDispose += _ =>
        {
            modifiers.Remove(modifier);
            Queries -= modifier.Handle;
            ModifiersChanged(this, new ModifierChangedArgs(modifier, ModifierChangedArgs.Change.Removed));
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
            if (mod.config.name == name)
            {
                modifier = mod;
                break;
            }
        }

        if (modifier == null) return false;

        if (modifier.Modify())
        {
            ModifiersChanged(this, new ModifierChangedArgs(modifier, ModifierChangedArgs.Change.Modified));
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ModifiersChanged(object sender, ModifierChangedArgs args)
    {
        OnModifierChanged.Invoke(sender, args);
    }

    private void ArrangeModifiers()
    {
        // Moves all multiplier modifiers to be calculated last
        foreach (var modifier in modifiers)
        {
            if (modifier.config.operatorType == StatModifier.OperatorType.Multiply)
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

public class ModifierChangedArgs : EventArgs
{
    public enum Change { Added, Removed, Modified }
    public readonly StatModifier modifier;
    public readonly Change change;

    public ModifierChangedArgs(StatModifier modifier, Change change)
    {
        this.modifier = modifier;
        this.change = change;
    }
}

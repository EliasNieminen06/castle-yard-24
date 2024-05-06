using System;
using UnityEngine;

public abstract class Unit : Entity, IDamageable
{
    [SerializeField] private BaseStats baseStats;
    public Stats Stats { get; private set; }
    public Health Health { get; private set; }

    public override void Init()
    {
        Stats = new Stats(new StatsMediator(), baseStats);
        Health = new Health(baseStats, Stats);

        Stats.Mediator.OnModifierChanged += OnModifiersChanged;
        Health.OnHealthChanged += OnHealthChanged;
        OnModifiersChanged(this, new ModifierChangedArgs(null, default));

        base.Init();
    }

    protected virtual void OnHealthChanged()
    {

    }

    protected virtual void OnModifiersChanged(object sender, ModifierChangedArgs args)
    {
        if (args.modifier == null) return;

        if (args.modifier.config.type == StatType.MaxHp)
        {
            if (args.change == ModifierChangedArgs.Change.Added)
            {
                Health.AddHp(args.modifier.config.value);
            }
            else if (args.change == ModifierChangedArgs.Change.Modified && args.modifier.config.stackable)
            {
                Health.AddHp(args.modifier.config.value);
            }

            Health.UpdateHp();
        }
    }

    public virtual void Update()
    {
        Stats.Mediator.Update();
    }

    public void ApplyStatModifier(StatModifier modifier)
    {
        Stats.Mediator.AddModifier(modifier);
    }

    public void Heal(float healAmount)
    {
        Health.AddHp(healAmount);
    }

    public void TakeDamage(float damageAmount)
    {
        Health.ReduceHp(damageAmount);
    }
}

using System;
using UnityEngine;

public enum StatType
{
    MaxHp,
    Attack,
    Defense,
    Speed,
    Magnet,
    [Tooltip("Precentage 1 = 100%")] CooldownReduction,
    [Tooltip("Precentage 1 = 100%")] AreaOfEffect,
    [Tooltip("Precentage 1 = 100%")] Dodge,
}

public class Stats
{
    readonly StatsMediator mediator;
    readonly BaseStats baseStats;

    public StatsMediator Mediator => mediator;

    public int MaxHp
    {
        get
        {
            // Return value with modifiers applied
            var q = new Query(StatType.MaxHp, baseStats.maxHp);
            mediator.PerformQuery(this, q);
            int roundedValue = Mathf.FloorToInt(q.Value);
            return roundedValue;
        }
    }

    public int Attack
    {
        get {
            // Return value with modifiers applied
            var q = new Query(StatType.Attack, baseStats.attack);
            mediator.PerformQuery(this, q);
            int roundedValue = Mathf.FloorToInt(q.Value);
            return roundedValue;
        }
    }

    public int Defense
    {
        get {
            // Return value with modifiers applied
            var q = new Query(StatType.Defense, baseStats.defense);
            mediator.PerformQuery(this, q);
            int roundedValue = Mathf.FloorToInt(q.Value);
            return roundedValue;
        }
    }

    public float Speed
    {
        get
        {
            // Return value with modifiers applied
            var q = new Query(StatType.Speed, baseStats.speed);
            mediator.PerformQuery(this, q);
            return q.Value;
        }
    }

    public float Magnet
    {
        get
        {
            // Return value with modifiers applied
            var q = new Query(StatType.Magnet, baseStats.magnet);
            mediator.PerformQuery(this, q);
            return q.Value;
        }
    }

    /// <summary>
    /// 0% - 100%
    /// </summary>
    public float CooldownReduction
    {
        get
        {
            // Return value with modifiers applied
            var q = new Query(StatType.CooldownReduction, baseStats.cooldownReduction);
            mediator.PerformQuery(this, q);
            float newValue = 100 * q.Value / (q.Value + 100);
            return q.Value;
        }
    }

    public Stats(StatsMediator mediator, BaseStats baseStats)
    {
        this.mediator = mediator;
        this.baseStats = baseStats;
    }

    public override string ToString() => $"Attack: {Attack}, Defense: {Defense}, Speed: {Math.Round(Speed, 2)}, Magnet: {Math.Round(Magnet, 2)}, CDR: {CooldownReduction}";
}

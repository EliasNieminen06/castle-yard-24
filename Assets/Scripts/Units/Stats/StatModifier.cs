using System;
using UnityEngine;

public class BasicStatModifier : StatModifier
{
    readonly Func<float, float> operation;

    public BasicStatModifier(StatModifierConfig modifierConfig) : base(modifierConfig)
    {
        operation = modifierConfig.operatorType switch
        {
            OperatorType.Add => (v) => v + currentValue,
            OperatorType.Multiply => (v) => v * currentValue,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override void Handle(object sender, Query query)
    {
        if (query.StatType == config.type)
        {
            query.Value = operation(query.Value);
        }
    }
}

public class StackableStatModifier : StatModifier
{
    // Maybe add different stacking methods

    public StackableStatModifier(StatModifierConfig modifierConfig) : base(modifierConfig)
    {

    }

    private float Operation(float v)
    {
        float valueAfterOperation;

        switch (config.operatorType)
        {
            case OperatorType.Add:
                valueAfterOperation = v + currentValue;
                break;
            case OperatorType.Multiply:
                valueAfterOperation = v * currentValue;
                break;
            default:
                valueAfterOperation = v + currentValue;
                break;
        }

        return valueAfterOperation;
    }

    public override void Handle(object sender, Query query)
    {
        if (query.StatType == config.type)
        {
            query.Value = Operation(query.Value);
        }
    }
}

public abstract class StatModifier : IDisposable
{
    public enum OperatorType {
        Add,
        Multiply
    }

    public readonly StatModifierConfig config;

    public float currentValue { get; private set; }

    private float timerEndTime;
    private float currentDuration;
    public float timeLeft => timerEndTime - Time.time;

    public bool MarkedForRemoval { get; private set; }

    public event Action<StatModifier> OnDispose = delegate { };

    public abstract void Handle(object sender, Query query);

    protected StatModifier(StatModifierConfig modifierConfig)
    {
        config = modifierConfig;

        currentValue = config.value;
        currentDuration = config.duration;

        if (config.duration > 0)
        {
            timerEndTime = Time.time + currentDuration;
        }
    }

    /// <summary>
    /// Returns true if modifier was modified
    /// </summary>
    public bool Modify()
    {
        bool modified = false;

        if (config.refreshable)
        {
            ResetTimer();
            modified = true;
        }
        if (config.stackable)
        {
            Stack();
            modified = true;
        }
        if (config.timeStackable)
        {
            AddTime(config.duration);
            modified = true;
        }

        if (modified) return true;
        else return false;
    }

    protected virtual void Stack()
    {
        if (config.stackValue == 0) currentValue += config.value;
        else currentValue += config.stackValue;
    }
    protected virtual void AddTime(float amount)
    {
        timerEndTime += amount;
        currentDuration = config.duration + amount;
    }
    private void ResetTimer()
    {
        timerEndTime = Time.time + currentDuration;
    }

    public void MarkForRemoval()
    {
        MarkedForRemoval = true;
    }

    public void Update()
    {
        if (currentDuration <= 0) return;
        if (timeLeft <= 0) MarkForRemoval();
    }

    public void Dispose()
    {
        OnDispose.Invoke(this);
    }
}

[System.Serializable]
public struct StatModifierConfig
{
    public readonly string name;
    public readonly StatType type;
    public readonly StatModifier.OperatorType operatorType;
    public readonly float value;
    public readonly float duration;
    public readonly bool stackable;
    public readonly float stackValue;
    public readonly bool refreshable;
    public readonly bool timeStackable;

    public StatModifierConfig(string name, StatType type, StatModifier.OperatorType operatorType, float value, float duration, bool stackable, float stackValue, bool refreshable, bool timeStackable)
    {
        this.name = name;
        this.type = type;
        this.operatorType = operatorType;
        this.value = value;
        this.duration = duration;
        this.stackable = stackable;
        this.stackValue = stackValue;
        this.refreshable = refreshable;
        this.timeStackable = timeStackable;
    }

    public StatModifierConfig(string name, StatType type, StatModifier.OperatorType operatorType, float value, float stackValue)
    {
        this.name = name;
        this.type = type;
        this.operatorType = operatorType;
        this.value = value;
        this.duration = 0;
        this.stackable = true;
        this.stackValue = stackValue;
        this.refreshable = false;
        this.timeStackable = false;
    }
}

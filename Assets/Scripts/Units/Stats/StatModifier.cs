using System;
using UnityEngine;

public class BasicStatModifier : StatModifier
{
    readonly Func<float, float> operation;

    public BasicStatModifier(BaseStatModifier baseModifier) : base(baseModifier, false)
    {
        operation = operatorType switch
        {
            OperatorType.Add => (v) => v + value,
            OperatorType.Multiply => (v) => v * value,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override void Handle(object sender, Query query)
    {
        if (query.StatType == type)
        {
            query.Value = operation(query.Value);
        }
    }
}

public class StackableStatModifier : StatModifier
{
    // Maybe add different stacking methods

    public StackableStatModifier(BaseStatModifier baseModifier) : base(baseModifier, true)
    {

    }

    private float Operation(float v)
    {
        float valueAfterOperation;

        switch (operatorType)
        {
            case OperatorType.Add:
                valueAfterOperation = v + value;
                break;
            case OperatorType.Multiply:
                valueAfterOperation = v * value;
                break;
            default:
                valueAfterOperation = v + value;
                break;
        }

        return valueAfterOperation;
    }

    public override void Handle(object sender, Query query)
    {
        if (query.StatType == type)
        {
            query.Value = Operation(query.Value);
        }
    }
}

public abstract class StatModifier : IDisposable
{
    public enum OperatorType { Add, Multiply }

    public readonly float baseValue;
    public float value { get; private set; }

    public readonly string name;
    protected readonly StatType type;
    public readonly OperatorType operatorType;

    readonly bool Stackable;
    readonly bool Refreshable;
    readonly bool timeStackable;

    private float timerEndTime;
    private float baseDuration;
    private float duration;
    public float timeLeft => timerEndTime - Time.time;

    public bool MarkedForRemoval { get; private set; }

    public event Action<StatModifier> OnDispose = delegate { };

    public abstract void Handle(object sender, Query query);

    protected StatModifier(BaseStatModifier baseModifier, bool stackable)
    {
        this.name = baseModifier.name;
        this.type = baseModifier.type;
        this.operatorType = baseModifier.operatorType;
        this.baseValue = baseModifier.value;
        this.value = baseValue;
        this.baseDuration = baseModifier.duration;
        this.duration = baseDuration;
        this.Stackable = stackable;
        this.Refreshable = baseModifier.refreshable;
        this.timeStackable = baseModifier.timeStackable;

        if (baseModifier.duration > 0)
        {
            timerEndTime = Time.time + duration;
        }
    }

    /// <summary>
    /// Returns true if modifier is modifyable and was modified
    /// </summary>
    public bool Modify()
    {
        bool modified = false;

        if (Refreshable)
        {
            ResetTimer();
            modified = true;
        }
        if (Stackable)
        {
            Stack();
            modified = true;
        }
        if (timeStackable)
        {
            AddTime(baseDuration);
            modified = true;
        }

        if (modified) return true;
        else return false;
    }

    protected virtual void Stack()
    {
        value += baseValue;
    }
    protected virtual void AddTime(float amount)
    {
        timerEndTime += amount;
        duration = baseDuration + amount;
    }
    private void ResetTimer()
    {
        timerEndTime = Time.time + duration;
    }

    public void MarkForRemoval()
    {
        MarkedForRemoval = true;
    }

    public void Update()
    {
        if (duration <= 0) return;
        if (timeLeft <= 0) MarkForRemoval();
    }

    public void Dispose()
    {
        OnDispose.Invoke(this);
    }

    public string GetName()
    {
        return name;
    }
}

using UnityEngine;

public abstract class Unit : Entity, IDamageable
{
    [SerializeField] private BaseStats baseStats;
    public Stats Stats { get; private set; }
    public Health Health { get; private set; }

    [SerializeField] private GameObject hpBarPrefab;
    private HpBar hpBar;

    public override void Init()
    {
        Stats = new Stats(new StatsMediator(), baseStats);
        Health = new Health(baseStats, Stats);

        Stats.Mediator.OnModifierChanged += OnModifiersChanged;
        Health.OnHealthChanged += OnHealthChanged;
        OnModifiersChanged(this, new ModifierChangedArgs(null, default));

        GameObject newHpBar = Instantiate(hpBarPrefab, GameManager.Instance.Canvas);
        hpBar = newHpBar.GetComponent<HpBar>();
        hpBar.Init(this.transform);

        base.Init();
    }

    protected virtual void OnHealthChanged()
    {
        hpBar.UpdateVisual(Health.currentHp, Stats.MaxHp);
    }

    protected virtual void OnModifiersChanged(object sender, ModifierChangedArgs args)
    {
        if (args.modifier == null) return;

        // ???

        if (args.modifier.config.type == StatType.MaxHp)
        {
            if (args.change == ModifierChangedArgs.Change.Added)
            {
                if (args.modifier.config.operatorType == StatModifier.OperatorType.Add)
                {
                    Health.AddHp(args.modifier.config.value);
                }
                else
                {
                    Health.AddHp(Health.currentHp * 1 + args.modifier.config.stackValue);
                }
            }
            else if (args.change == ModifierChangedArgs.Change.Modified && args.modifier.config.stackable)
            {
                if (args.modifier.config.operatorType == StatModifier.OperatorType.Add)
                {
                    Health.AddHp(args.modifier.config.value);
                }
                else
                {
                    Health.AddHp(Health.currentHp * 1 + args.modifier.config.stackValue);
                }
            }

            Health.UpdateHp();
        }

        // ???
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
        bool dodge = Random.Range(0, 100) < Stats.Dodge;
        if (dodge)
        {
            Debug.Log("Dodged");
            return;
        }

        float damageToTake = DamageCalculator.CalculateDamage(damageAmount, Stats.Defense);
        Debug.Log($"Took {damageToTake} damage");

        Health.ReduceHp(damageToTake);
    }
}

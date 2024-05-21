using System.Collections;
using TMPro;
using UnityEngine;

public abstract class Unit : Entity, IDamageable
{
    [SerializeField] private BaseStats baseStats;
    public Stats Stats { get; private set; }
    public Health Health { get; private set; }

    [SerializeField] private GameObject hpBarPrefab;
    private HpBar hpBar;

    private Vector3 originalScale;

    [SerializeField] private GameObject damageTextPopupPrefab;
    public override void Init()
    {
        originalScale = transform.localScale;

        Stats = new Stats(new StatsMediator(), baseStats);
        Health = new Health(baseStats, Stats);

        Stats.Mediator.OnModifierChanged += OnModifiersChanged;
        Health.OnHealthChanged += OnHealthChanged;
        OnModifiersChanged(this, new ModifierChangedArgs(null, default));

        if (hpBarPrefab != null)
        {
            GameObject newHpBar = Instantiate(hpBarPrefab, GameManager.Instance.Canvas);
            hpBar = newHpBar.GetComponent<HpBar>();
            hpBar.Init(this.transform);
        }

        base.Init();
    }

    protected virtual void OnHealthChanged()
    {
        if (hpBar != null) hpBar.UpdateVisual(Health.currentHp, Stats.MaxHp);
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
        Vector3 textPopupPosition = new Vector3(transform.position.x + Random.Range(-0.2f, 0.2f), transform.position.y + Mathf.Clamp(transform.localScale.y + 0.75f, 0, 4), transform.position.z + 0.5f);

        bool dodge = Random.Range(0, 100) < Stats.Dodge;
        if (dodge)
        {
            GameObject newText1 = Instantiate(damageTextPopupPrefab, textPopupPosition, Quaternion.identity);
            newText1.GetComponent<TextMeshPro>().text = "dodge";
            //  Debug.Log(this + " Dodged");
            return;
        }

        float damageToTake = DamageCalculator.CalculateDamage(damageAmount, Stats.Defense);
        Health.ReduceHp(damageToTake);

        StartCoroutine(SetScaleInSeconds(originalScale, 0.035f));
        transform.localScale *= 1.1f;

        GameObject newText = Instantiate(damageTextPopupPrefab, textPopupPosition, Quaternion.identity);
        newText.GetComponent<TextMeshPro>().text = System.Math.Round(damageToTake, 1).ToString();

        //Debug.Log($"{this} Took {damageToTake} damage, Hp Left: {Health.currentHp}");

        if (Health.currentHp <= 0) OnDeath();

        IEnumerator SetScaleInSeconds(Vector3 scale, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            transform.localScale = scale;
        }
    }

    protected virtual void OnDeath() {
        //Debug.Log("Unit.OnDeath " + this);
        if (hpBar != null) Destroy(hpBar.gameObject);
    }
}

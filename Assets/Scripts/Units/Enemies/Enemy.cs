using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit, IVisitor
{
    [SerializeField] List<itemDrop> dropTable;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] private float attackFrequency;

    [Header("Levels")]
    [SerializeField] private float healthPerLevel;
    [SerializeField] private float attackPerLevel;
    [SerializeField] private float defensePerLevel;
    [SerializeField] private float speedPerLevel;

    protected Transform player;

    private float nextAttack;

    private IVisitable visitable;

    public override void Init()
    {
        player = GameManager.Instance.Player;
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody On " + this.gameObject + " is null");
            rb = GetComponent<Rigidbody>();
        }

        base.Init();

        int level = WaveSpawner.Instance.currentWave - 1;

        if (level == 0) return;

        Stats.Mediator.AddModifier(new BasicStatModifier(new StatModifierConfig("HealthBuff", StatType.MaxHp, StatModifier.OperatorType.Add, healthPerLevel * level, "HP")));
        Stats.Mediator.AddModifier(new BasicStatModifier(new StatModifierConfig("AttackBuff", StatType.Attack, StatModifier.OperatorType.Add, attackPerLevel * level, "ATK")));
        Stats.Mediator.AddModifier(new BasicStatModifier(new StatModifierConfig("DefenseBuff", StatType.Defense, StatModifier.OperatorType.Add, defensePerLevel * level, "DEF")));
        Stats.Mediator.AddModifier(new BasicStatModifier(new StatModifierConfig("SpeedBuff", StatType.Speed, StatModifier.OperatorType.Add, speedPerLevel * level, "SPD")));
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TakeDamage(10f);
        }

        base.Update();
    }

    public void Visit<T>(T visitable) where T : Component, IVisitable
    {
        if (visitable is Hero hero)
        {
            hero.TakeDamage(Stats.Attack);
            nextAttack = Time.time + attackFrequency;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Time.time < nextAttack) return;
        if (visitable == null) visitable = other.GetComponentInParent<IVisitable>();

        visitable?.Accept(this);
    }

    protected override void OnDeath()
    {
        // drop items before destroying
        foreach (var item in dropTable)
        {
            int rng = Random.Range(0, 100);
            if (rng < item.chance)
            {
                Instantiate(item.prefab, transform.position, Quaternion.identity);
            }
        }

        base.OnDeath();

        Destroy(gameObject);
    }

    [System.Serializable]
    private struct itemDrop
    {
        public GameObject prefab;
        public int chance;
    }
}

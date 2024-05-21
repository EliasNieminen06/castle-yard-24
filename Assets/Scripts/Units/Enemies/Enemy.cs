using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit, IVisitor
{
    [SerializeField] List<itemDrop> dropTable;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected float attackFrequency;

    [Header("Levels")]
    [SerializeField] private float healthPerLevel;
    [SerializeField] private float attackPerLevel;
    [SerializeField] private float defensePerLevel;
    [SerializeField] private float speedPerLevel;
    [SerializeField] private float CDRPerLevel;

    protected Transform player;

    private float nextAttack;

    private IVisitable visitable;

    [SerializeField] private bool isChest;

    public override void Init()
    {
        player = GameManager.Instance.Player;
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody On " + this.gameObject + " is null");
            rb = GetComponent<Rigidbody>();
        }

        if (isChest) rb.AddForce(Random.insideUnitSphere.normalized * 5f, ForceMode.Impulse);

        base.Init();

        int level = WaveSpawner.Instance.currentWave - 1;

        if (level == 0) return;

        Stats.Mediator.AddModifier(new BasicStatModifier(new StatModifierConfig("HealthBuff", StatType.MaxHp, StatModifier.OperatorType.Add, healthPerLevel * level)));
        Stats.Mediator.AddModifier(new BasicStatModifier(new StatModifierConfig("AttackBuff", StatType.Attack, StatModifier.OperatorType.Add, attackPerLevel * level)));
        Stats.Mediator.AddModifier(new BasicStatModifier(new StatModifierConfig("DefenseBuff", StatType.Defense, StatModifier.OperatorType.Add, defensePerLevel * level)));
        Stats.Mediator.AddModifier(new BasicStatModifier(new StatModifierConfig("SpeedBuff", StatType.Speed, StatModifier.OperatorType.Add, speedPerLevel * level)));
        Stats.Mediator.AddModifier(new BasicStatModifier(new StatModifierConfig("CDRBuff", StatType.CooldownReduction, StatModifier.OperatorType.Add, CDRPerLevel * level)));
    }

    public override void Update()
    {
        base.Update();
    }

    public void Visit<T>(T visitable) where T : Component, IVisitable
    {
        if (visitable is Hero hero)
        {
            hero.TakeDamage(Stats.Attack);
            nextAttack = Time.time + attackFrequency * (1 - Stats.CooldownReduction / 100);
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
                Instantiate(item.prefab, transform.position + Vector3.up * Mathf.Clamp(transform.localScale.y, 0f, 3f), Quaternion.identity);
            }
        }

        base.OnDeath();

        KillCounter.AddKill_Static();
        Destroy(gameObject);
    }

    [System.Serializable]
    private struct itemDrop
    {
        public GameObject prefab;
        public int chance;
    }
}

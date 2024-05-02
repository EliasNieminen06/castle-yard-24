using System;
using UnityEngine;
using TMPro;

public class Hero : Unit, IVisitable
{
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI levelsText;
    public TextMeshProUGUI healthText;

    public Levels Levels { get; private set; }

    public override void Init()
    {
        Levels = new Levels(100, 10, 10);
        Levels.OnLevelUp += OnLevelUp;

        base.Init();
    }

    protected virtual void OnLevelUp(int level)
    {
        Health.SetHpToMax();
        UpdateVisuals();

        // TODO popup levelup screen to choose powerup
    }

    protected override void OnHealthChanged()
    {
        base.OnHealthChanged();
        UpdateVisuals();
    }

    protected override void OnModifiersChanged()
    {
        base.OnModifiersChanged();
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        statsText.text = Stats.ToString();
        healthText.text = Health.ToString();
    }

    private void Awake()
    {
        // Temporary
        Init();
    }

    public void AddExperience(int amount)
    {
        Levels.AddExperience(amount);
    }

    public override void Update()
    {
        levelsText.text = Levels.ToString();
        base.Update();
    }

    public void Accept(IVisitor visitor) => visitor.Visit(this);
}

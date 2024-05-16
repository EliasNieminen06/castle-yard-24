﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Hero : Unit, IVisitable
{
    public Levels Levels { get; private set; }

    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private Image expBar;
    [SerializeField] private TextMeshProUGUI levelText;

    public override void Init()
    {
        Levels = new Levels(expBar, 100, 10, 10);
        Levels.OnLevelUp += OnLevelUp;

        base.Init();
    }

    protected virtual void OnLevelUp(int level)
    {
        Health.SetHpToMax();
        levelText.text = "Lv: " + Levels.level;

        LevelUpScreen.Show_Static(this, () => { Levels.CheckExperience(); });
    }

    protected override void OnHealthChanged()
    {
        base.OnHealthChanged();
    }

    protected override void OnModifiersChanged(object sender, ModifierChangedArgs args)
    {
        base.OnModifiersChanged(sender, args);
        StatsAndHealthText.UpdateTexts_Static();

        if (GetComponent<PlayerMovementHandler>() != null)
        {
            this.GetComponent<PlayerMovementHandler>().ModifySpeed();
        }
    }

    public void AddExperience(int amount)
    {
        float newAmount = amount * ((float)Stats.ExpBonus / 100 + 1);
        int roundedAmount = Mathf.RoundToInt(newAmount);
        Levels.AddExperience(roundedAmount);
        //Debug.Log("Added " + roundedAmount + " xp");
    }

    public override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        AttractPickups();
    }

    private void AttractPickups()
    {
        Collider[] collidersToAttract = Physics.OverlapSphere(transform.position, Stats.Magnet, pickupLayer);

        foreach (var collider in collidersToAttract)
        {
            if (collider == null) continue;

            float distanceToPickup = (transform.position - collider.transform.position).magnitude;
            float pickupAttractionDivisor = Helpers.Map(distanceToPickup, 0f, Stats.Magnet, 1f, 4f, false);

            collider.transform.position = Vector3.MoveTowards(collider.transform.position, transform.position, 1f / pickupAttractionDivisor);
        }
    }

    public void Accept(IVisitor visitor) => visitor.Visit(this);
}

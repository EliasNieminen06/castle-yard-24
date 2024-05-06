using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hero : Unit, IVisitable
{
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI levelsText;
    public TextMeshProUGUI healthText;

    public Levels Levels { get; private set; }

    [SerializeField] LayerMask pickupLayer;

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

        LevelUpScreen.Show_Static(this, () => { Levels.CheckExperience(); UpdateVisuals(); });
    }

    protected override void OnHealthChanged()
    {
        base.OnHealthChanged();
        UpdateVisuals();
    }

    protected override void OnModifiersChanged(object sender, ModifierChangedArgs args)
    {
        base.OnModifiersChanged(sender, args);
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        statsText.text = Stats.ToString();
        healthText.text = Health.ToString();

        if(GetComponent<PlayerMovementHandler>() != null)
        {
            this.GetComponent<PlayerMovementHandler>().ModifySpeed();
        }
        

        
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
            float pickupAttractionDelta = Helpers.Map(distanceToPickup, 0f, Stats.Magnet, 1f, 4f, false);

            collider.transform.position = Vector3.MoveTowards(collider.transform.position, transform.position, 1f / pickupAttractionDelta);
        }
    }

    public void Accept(IVisitor visitor) => visitor.Visit(this);
}

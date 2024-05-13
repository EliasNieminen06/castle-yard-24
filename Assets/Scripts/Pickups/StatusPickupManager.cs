using System;
using UnityEngine;
using System.Collections.Generic;

public class StatPickupManager : MonoBehaviour
{
    private static StatPickupManager instance;

    [SerializeField] GameObject pickupUITemplate;
    [SerializeField] Transform panel;

    private Dictionary<StatModifierPickupBase, StatusEffectUI> modifierDictionary = new Dictionary<StatModifierPickupBase, StatusEffectUI>();

    private void Awake()
    {
        instance = this;
    }

    private void AddPickup(StatModifierPickupBase pickupBase)
    {
        GameObject newPickupUI = Instantiate(pickupUITemplate, panel);
        StatusEffectUI effectUI = newPickupUI.GetComponent<StatusEffectUI>();

        modifierDictionary.Add(pickupBase, effectUI);

        effectUI.Init(pickupBase.duration, pickupBase.color, pickupBase.value, pickupBase.operatorType);
        effectUI.OnRemove += _ =>
        {
            modifierDictionary.Remove(pickupBase);
        };
    }

    private void ModifyPickup(StatModifierPickupBase pickupBase, bool refresh, float addedTime)
    {
        if (!modifierDictionary.TryGetValue(pickupBase, out StatusEffectUI effectUI))
        {
            Debug.LogWarning("Tried modifying an effectUI that doesnt exist");
            return;
        }

        effectUI.Modify(refresh, addedTime, pickupBase.stackable, pickupBase.operatorType, pickupBase.stackValue);
    }

    public static void AddPickup_Static(StatModifierPickupBase pickupBase)
    {
        instance.AddPickup(pickupBase);
    }

    public static void ModifyPickup_Static(StatModifierPickupBase pickupBase, bool refresh, float addedTime)
    {
        instance.ModifyPickup(pickupBase, refresh, addedTime);
    }
}



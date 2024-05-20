using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeItem", menuName = "Stats/UpgradeItem")]
public class UpgradeItem : ScriptableObject
{
    public string itemName;
    public StatType type;
    public StatModifier.OperatorType operatorType;
    public float value;
    public float stackValue;
    public int maxStacks;
    public int weight;
    public Color color;
    public string description;
    public Sprite icon;

    [HideInInspector] public StatModifierConfig modifierConfig;

    private void OnEnable()
    {
        modifierConfig = new StatModifierConfig(itemName, type, operatorType, value, stackValue, maxStacks, description, icon);
    }
}

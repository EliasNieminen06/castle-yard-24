using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "StatUpgrade", menuName = "Stats/StatUpgrade")]
public class StatUpgrade : ScriptableObject
{
    public string upgradeName;
    public StatType type;
    public StatModifier.OperatorType operatorType;
    public int weight;

    [Header("Values")]
    public float commonValue;
    public float rareValue;
    public float epicValue;
    public float legendaryValue;

    [Header("StackValues")]
    public float commonStackValue;
    public float rareStackValue;
    public float epicStackValue;
    public float legendaryStackValue;

    [Header("Weights")]
    public int commonWeight;
    public int rareWeight;
    public int epicWeight;
    public int legendaryWeight;

    [HideInInspector] public StatModifierConfig common;
    [HideInInspector] public StatModifierConfig rare;
    [HideInInspector] public StatModifierConfig epic;
    [HideInInspector] public StatModifierConfig legendary;

    public int TotalWeight()
    {
        return commonWeight + rareWeight + epicWeight + legendaryWeight;
    }

    private void OnEnable()
    {
        common = new StatModifierConfig(upgradeName + "Common", type, operatorType, commonValue, commonStackValue);
        rare = new StatModifierConfig(upgradeName + "Rare", type, operatorType, rareValue, rareStackValue);
        epic = new StatModifierConfig(upgradeName + "Epic", type, operatorType, epicValue, epicStackValue);
        legendary = new StatModifierConfig(upgradeName + "Legendary", type, operatorType, legendaryValue, legendaryStackValue);
    }
}

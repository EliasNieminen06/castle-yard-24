using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "StatUpgrade", menuName = "Stats/StatUpgrade")]
public class StatUpgrade : ScriptableObject
{
    public string upgradeName;
    public StatType type;
    public StatModifier.OperatorType operatorType;

    [Header("Values")]
    public float commonValue;
    public float rareValue;
    public float epicValue;
    public float legendaryValue;

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
        common = new StatModifierConfig(upgradeName + "Common", type, operatorType, commonValue);
        rare = new StatModifierConfig(upgradeName + "Rare", type, operatorType, rareValue);
        epic = new StatModifierConfig(upgradeName + "Epic", type, operatorType, epicValue);
        legendary = new StatModifierConfig(upgradeName + "Legendary", type, operatorType, legendaryValue);
    }
}

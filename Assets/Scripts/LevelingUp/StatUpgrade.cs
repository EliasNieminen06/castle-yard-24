using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "StatUpgrade", menuName = "Stats/StatUpgrade")]
public class StatUpgrade : ScriptableObject
{
    public string upgradeName;
    public StatType type;
    public StatModifier.OperatorType operatorType;
    public int weight;

    public Sprite icon;

    [Header("Values")]
    public float commonValue;
    public float rareValue;
    public float epicValue;
    public float legendaryValue;


    [Header("ValueStrings")]
    public string commonValueString;
    public string rareValueString;
    public string epicValueString;
    public string legendaryValueString;

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
        common = new StatModifierConfig(upgradeName + "Common", type, operatorType, commonValue, commonValueString, icon);
        rare = new StatModifierConfig(upgradeName + "Rare", type, operatorType, rareValue, rareValueString, icon);
        epic = new StatModifierConfig(upgradeName + "Epic", type, operatorType, epicValue, epicValueString, icon);
        legendary = new StatModifierConfig(upgradeName + "Legendary", type, operatorType, legendaryValue, legendaryValueString, icon);
    }
}

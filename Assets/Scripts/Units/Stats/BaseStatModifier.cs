using UnityEngine;

[CreateAssetMenu(fileName = "StatModifier", menuName = "Stats/StatModifier")]
public class BaseStatModifier : ScriptableObject
{
    public string modifierName;
    public StatType type;
    public StatModifier.OperatorType operatorType;
    public float value;
    public float duration;
    public bool stackable;
    public bool refreshable;
    public bool timeStackable;
}

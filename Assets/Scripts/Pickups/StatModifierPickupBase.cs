using UnityEngine;

[CreateAssetMenu(fileName = "StatModifierPickup", menuName = "Stats/StatModifierPickup")]
public class StatModifierPickupBase : ScriptableObject
{
    public string modifierName;
    public StatType type;
    public StatModifier.OperatorType operatorType;
    public float value;
    public float duration;
    public bool stackable;
    public bool refreshable;
    public bool timeStackable;

    public StatModifierConfig config { get; private set; }

    private void OnEnable()
    {
        config = new StatModifierConfig(modifierName, type, operatorType, value, duration, stackable, refreshable, timeStackable);
    }
}

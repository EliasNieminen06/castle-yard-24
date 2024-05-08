using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseStats", menuName = "Stats/BaseStats")]
public class BaseStats : ScriptableObject
{
    public int maxHp;
    public int attack;
    public int defense;
    public float speed;
    public float magnet;
    public float cooldownReduction;
    public float areaOfEffect;
    public float dodge;
}

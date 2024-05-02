using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float damageAmount);
    public void ApplyStatModifier(StatModifier modifier);
}

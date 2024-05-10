public static class DamageCalculator
{
    public static float CalculateDamage(float damage, int defense)
    {
        float damageReduction = 0.9f - 0.9f / (1f + 0.01f * defense);
        float damageToReduce = damage * damageReduction;
        float newDamage = damage - damageToReduce;
        return newDamage;
    }
}

public class ConditionalDamage : Effect
{
    int normalDamage;
    int conditionalDamage;
    Condition condition;
    public ConditionalDamage(int normalDamage, int conditionalDamage, bool useOnHostile, bool useOnFriendly, Condition condition) : base((int)EffectIndex.Damage, useOnHostile, useOnFriendly)
    {
        this.normalDamage = normalDamage;
        this.conditionalDamage = conditionalDamage;
        this.condition = condition;
    }
    public override string GetDescription()
    {
        return $"Dealing {normalDamage} damage to vitality. If target is {condition.ToString()} damage is increased to {conditionalDamage}";
    }
    public override void ApplyEffect(Character attacker, Character target, SkillcheckSystem.CombatOutcome outcome, bool hostile)
    {
        if (IsValidEffectTarget(hostile))
        {
            bool affectedByCondition = target.HasCondition(condition);

            if (affectedByCondition)
            {
                target.characterData.Vitality.CurrentValue -= GetModifiedValue(outcome, conditionalDamage);
            }
            else
            {
                target.characterData.Vitality.CurrentValue -= GetModifiedValue(outcome, normalDamage);
            }
        }
    }
}

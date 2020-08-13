public class StackDamage : Effect
{
    int damagePerStack;
    Condition condition;
    public StackDamage(int damagePerStack, bool useOnHostile, bool useOnFriendly, Condition condition) : base((int)EffectIndex.Damage, useOnHostile, useOnFriendly)
    {
        this.damagePerStack = damagePerStack;
        this.condition = condition;
    }
    public override string GetDescription()
    {
        return $"Dealing damage to a {condition.ToString()} character. The damage equals {damagePerStack} times the number of such effects";
    }
    public override void ApplyEffect(Character attacker, Character target, bool crit, bool hostile)
    {
        if (IsValidEffectTarget(hostile))
        {
            int numberOfConditions = target.NumberOfConditions(condition);

            int damage = damagePerStack * numberOfConditions;

            if (numberOfConditions > 0)
            {
                target.characterData.Vitality.CurrentValue -= GetModifiedValue(crit, damage);
            }
        }
    }
}

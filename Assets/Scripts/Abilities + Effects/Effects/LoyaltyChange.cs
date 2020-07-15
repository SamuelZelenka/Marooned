public class LoyaltyDecrease : Effect
{
    int amount;
    public LoyaltyDecrease(int amount, bool useOnHostile, bool useOnFriendly) : base((int)EffectIndex.LoyaltyDecrease, useOnHostile, useOnFriendly)
    {
        this.amount = amount;
    }

    public override void ApplyEffect(Character attacker, Character target, SkillcheckSystem.CombatOutcome outcome, bool hostile)
    {
        if (IsValidEffectTarget(hostile))
        {
            target.characterData.Loyalty.CurrentValue -= GetModifiedValue(outcome, amount);
        }
    }
    public override string GetDescription()
    {
        return $"Loyalty reduced by {amount}";
    }
}

public class LoyaltyIncrease : Effect
{
    int amount;
    public LoyaltyIncrease(int amount, bool useOnHostile, bool useOnFriendly) : base((int)EffectIndex.LoyaltyIncrease, useOnHostile, useOnFriendly)
    {
        this.amount = amount;
    }

    public override void ApplyEffect(Character attacker, Character target, SkillcheckSystem.CombatOutcome outcome, bool hostile)
    {
        if (IsValidEffectTarget(hostile))
        {
            target.characterData.Loyalty.CurrentValue += GetModifiedValue(outcome, amount);
        }
    }
    public override string GetDescription()
    {
        return $"Increases loyalty by {amount}";
    }
}

public class LoyaltyDecrease : Effect
{
    int amount;
    public LoyaltyDecrease(int amount) : base((int)EffectIndex.LoyaltyDecrease)
    {
        GetDescription();
        this.amount = amount;
    }

    public override void ApplyEffect(Character attacker, Character target, SkillcheckSystem.CombatOutcome outcome)
    {
        target.characterData.Loyalty.CurrentValue -= GetModifiedValue(outcome, amount);
    }
    public override string GetDescription()
    {
        return Description = $"Loyalty reduced by {amount}";
    }
}

public class LoyaltyIncrease : Effect
{
    int amount;
    public LoyaltyIncrease(int amount) : base((int)EffectIndex.LoyaltyIncrease)
    {
        GetDescription();
        this.amount = amount;
    }

    public override void ApplyEffect(Character attacker, Character target, SkillcheckSystem.CombatOutcome outcome)
    {
        target.characterData.Loyalty.CurrentValue += GetModifiedValue(outcome, amount);
    }
    public override string GetDescription()
    {
       return Description = $"Increases loyalty by {amount}";
    }
}

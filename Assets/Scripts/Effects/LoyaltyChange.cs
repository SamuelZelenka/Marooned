public class LoyaltyDecrease : Effect
{
    int amount;
    public LoyaltyDecrease(int amount) : base((int)EffectIndex.LoyaltyDecrease)
    {
        Description = $"Reduces loyalty by {amount}";
        this.amount = amount;
    }

    public override void ApplyEffect(Character attacker, Character target, SkillcheckSystem.CombatOutcome outcome)
    {
        target.characterData.Loyalty.CurrentValue -= GetModifiedValue(outcome, amount);
    }
}

public class LoyaltyIncrease : Effect
{
    int amount;
    public LoyaltyIncrease(int amount) : base((int)EffectIndex.LoyaltyIncrease)
    {
        Description = $"Increases loyalty by {amount}";
        this.amount = amount;
    }

    public override void ApplyEffect(Character attacker, Character target, SkillcheckSystem.CombatOutcome outcome)
    {
        target.characterData.Loyalty.CurrentValue += GetModifiedValue(outcome, amount);
    }
}

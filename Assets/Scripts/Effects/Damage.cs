public class Damage : Effect
{
    int damage;
    public Damage(int damage) : base((int)EffectIndex.Damage)
    {
        GetDescription();
        this.damage = damage;
    }
    public override string GetDescription()
    {
        return Description = $"Reduced {damage} vitality";
    }
    public override void ApplyEffect(Character attacker, Character target, SkillcheckSystem.CombatOutcome outcome)
    {
        target.characterData.Vitality.CurrentValue -= GetModifiedValue(outcome, damage);
    }
}

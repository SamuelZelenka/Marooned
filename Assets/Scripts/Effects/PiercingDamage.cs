public class PiercingDamage : Effect
{
    int damage;
    public PiercingDamage(int damage) : base((int)EffectIndex.Damage)
    {
        Description = $"Reduce {damage} vitality, ignoring fortitude";
        this.damage = damage;
    }
    public override void ApplyEffect(Character attacker, Character target, SkillcheckSystem.CombatOutcome outcome)
    {
        target.characterData.Vitality.CurrentValue -= GetModifiedValue(outcome, damage);
    }
}

public class PiercingDamage : Effect
{
    int damage;
    public PiercingDamage(int damage, bool useOnHostile, bool useOnFriendly) : base((int)EffectIndex.Damage, useOnHostile, useOnFriendly)
    {
        this.damage = damage;
    }
    public override void ApplyEffect(Character attacker, Character target, SkillcheckSystem.CombatOutcome outcome, bool hostile)
    {
        if (IsValidEffectTarget(hostile))
        {
            target.characterData.Vitality.CurrentValue -= GetModifiedValue(outcome, damage);
        }
    }
    public override string GetDescription()
    {
        return $"Reduce {damage} vitality, ignoring fortitude";
    }
}

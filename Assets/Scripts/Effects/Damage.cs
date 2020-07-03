public class Damage : Effect
{
    int damage;
    public Damage(int damage)
    {
        Description = $"Reduces {damage} vitality.";
        this.damage = damage;
    }
    public override void ApplyEffect(Character attacker, Character target, SkillcheckSystem.CombatOutcome outcome)
    {
        target.characterData.Vitality.CurrentValue -= GetModifiedValue(outcome, damage);
    }
}

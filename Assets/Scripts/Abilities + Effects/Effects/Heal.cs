public class Heal : Effect
{
    int heal;
    public Heal(int heal, bool useOnHostile, bool useOnFriendly) : base((int)EffectIndex.Heal, useOnHostile, useOnFriendly)
    {
        this.heal = heal;
    }
    public override string GetDescription()
    {
        return $"Increase {heal} vitality";
    }
    public override void ApplyEffect(Character attacker, Character target, bool crit, bool hostile)
    {
        if (IsValidEffectTarget(hostile))
        {
            target.characterData.Vitality.CurrentValue += GetModifiedValue(crit, heal);
        }
    }
}

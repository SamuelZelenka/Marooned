public class Damage : Effect
{
    int damage;
    public Damage(int damage, bool useOnHostile, bool useOnFriendly) : base((int)EffectIndex.Damage, useOnHostile, useOnFriendly)
    {
        this.damage = damage;
    }
    public override string GetDescription()
    {
        return $"Reduced {damage} vitality";
    }
    public override void ApplyEffect(Character attacker, Character target, bool crit, bool hostile)
    {
        if (IsValidEffectTarget(hostile))
        {
            target.characterData.Vitality.CurrentValue -= GetModifiedValue(crit, damage);
        }
    }
}

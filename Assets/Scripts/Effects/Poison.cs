public class Poison : TickEffect
{
    int damage;
    public Poison(int damage, int duration, bool useOnHostile, bool useOnFriendly) : base((int)EffectIndex.Poison, useOnHostile, useOnFriendly, duration)
    {
        this.damage = damage;
    }
    public override void EffectTick(Character owner)
    {
        owner.characterData.Vitality.CurrentValue -= damage;
        base.EffectTick(owner);
    }
    public override string GetDescription()
    {
        if (duration == 1)
        {
            return $"Vitality reduced by {damage} for {duration - counter + 1} turn";
        }
            return $"Vitality reduced by {damage} for {duration - counter + 1} turns";
    }
}
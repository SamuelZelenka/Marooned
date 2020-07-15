public class Bleed : TickEffect
{
    int damage;
    public Bleed(int damage, int duration, bool useOnHostile, bool useOnFriendly) : base((int)EffectIndex.Bleed, useOnHostile, useOnFriendly, duration)
    {
        this.damage = damage;
    }
    public override string GetDescription()
    {
        if (duration == 1)
        {
            return $"Vitality reduced by {damage} for {duration - counter + 1} turn";
        }
           return $"Vitality reduced by {damage} for {duration - counter + 1} turns";
    }

    public override void EffectTick(Character owner)
    {
        owner.characterData.Vitality.CurrentValue -= damage;
        base.EffectTick(owner);
    }
}
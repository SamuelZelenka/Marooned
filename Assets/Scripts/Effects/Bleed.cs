public class Bleed : TickEffect
{
    int damage;
    public Bleed(int damage, int duration) : base((int)EffectIndex.Bleed)
    {
        GetDescription();
        this.damage = damage;
        base.duration = duration;
    }
    public override string GetDescription()
    {
        if (duration == 1)
        {
            return Description = $"Vitality reduced by {damage} for {duration - counter + 1} turn";
        }
           return Description = $"Vitality reduced by {damage} for {duration - counter + 1} turns";
    }

    public override void EffectTick(Character target)
    {
        target.characterData.Vitality.CurrentValue -= damage;
        base.EffectTick(target);
    }
}
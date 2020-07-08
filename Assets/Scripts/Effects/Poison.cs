public class Poison : TickEffect
{
    int damage;
    public Poison(int damage, int duration) : base((int)EffectIndex.Poison)
    {

        this.damage = damage;
        base.duration = duration;
    }
    public override void EffectTick(Character target)
    {
        target.characterData.Vitality.CurrentValue -= damage;
        base.EffectTick(target);
    }
    public override string GetDescription()
    {
        if (duration == 1)
        {
            return Description = $"Vitality reduced by {damage} for {duration - counter + 1} turn";
        }
            return Description = $"Vitality reduced by {damage} for {duration - counter + 1} turns";
    }
}
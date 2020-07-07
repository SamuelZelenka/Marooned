public class Poison : TickEffect
{
    int damage;
    public Poison(int damage, int duration) : base((int)EffectIndex.Poison)
    {
        if (duration == 1)
        {
            Description = $"Vitality reduced by {damage} for {duration} turn";
        }
        else
        {
            Description = $"Vitality reduced by {damage} for {duration} turns";
        }
        this.damage = damage;
        base.duration = duration;
    }
    public override void EffectTick(Character target)
    {
        target.characterData.Vitality.CurrentValue -= damage;
        base.EffectTick(target);
    }
}
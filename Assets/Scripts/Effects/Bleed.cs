public class Bleed : TickEffect
{
    int damage;
    public Bleed(int damage, int duration)
    {
        if (duration == 1)
        {
            Description = $"Reduces the target's vitality with {damage} for {duration} turn";
        }
        else
        {
            Description = $"Reduces the target's vitality with {damage} for {duration} turns";
        }
        this.damage = damage;
        base.duration = duration;
    }
    public override void EffectTick(Character character)
    {
        character.characterData.Vitality.CurrentValue -= damage;
        base.EffectTick(character);
    }
}
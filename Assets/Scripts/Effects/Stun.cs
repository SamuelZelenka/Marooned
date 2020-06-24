public class Stun : TickEffect
{
    public Stun(int duration)
    {
        if (duration == 1)
        {
            Description = $"Stuns the target for {duration} turn";
        }
        else
        {
            Description = $"Stuns the target for {duration} turns";
        }
        base.duration = duration;
    }
    public override void ApplyEffect(Character character)
    {
        base.ApplyEffect(character);
        character.isStunned = true;
    }
    public override void EffectTick(Character character)
    {
        base.EffectTick(character);
    }
    public override void RemoveEffect(Character character)
    {
        base.RemoveEffect(character);
        foreach (Effect effect in character.characterData.activeEffects)
        {
            if (effect.GetType() == this.GetType())
            {
                return;
            }
        }
        character.isStunned = false;
    }
}
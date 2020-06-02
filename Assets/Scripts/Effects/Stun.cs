public class Stun : Effect
{
    int duration;
    int counter;
    public Stun(int duration)
    {
        if (duration == 1)
        {
            description = $"Stuns the enemy for {duration} turn";

        }
        else
        {
            description = $"Stuns the enemy for {duration} turns";

        }
        this.duration = duration;
    }
    public override void ApplyEffect(Character character) {}
    public override void EffectTick(Character character)
    {
        if (counter < duration)
        {
            character.isStunned = true;
        }
        else
        {
            RemoveEffect(character);
        }
        counter++;
    }
    public void RemoveEffect(Character character)
    {
        character.RemoveEffects(this);
        foreach (Effect effect in character.activeEffects)
        {
            if (effect.GetType() == this.GetType())
            {
                return;
            }
        }
        character.isStunned = false;
    }
}
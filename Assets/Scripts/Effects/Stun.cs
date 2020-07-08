public class Stun : TickEffect
{
    public Stun(int duration) : base((int) EffectIndex.Stun)
    {

        base.duration = duration;
    }
    public override void ApplyEffect(Character attacker, Character target, SkillcheckSystem.CombatOutcome outcome)
    {
        base.ApplyEffect(attacker, target, outcome);
        target.isStunned = true;
    }
    public override void EffectTick(Character target)
    {
        base.EffectTick(target);
    }
    public override void RemoveEffect(Character target)
    {
        base.RemoveEffect(target);
        foreach (Effect effect in target.characterData.activeEffects)
        {
            if (effect.GetType() == this.GetType())
            {
                return;
            }
        }
        target.isStunned = false;
    }
    public override string GetDescription()
    {
        if (duration == 1)
        {
           return Description = $"Stunned for {duration - counter + 1} turn";
        }
           return Description = $"Stunned for {duration - counter + 1} turns";
    }
}
public class Taunt : TickEffect
{
    public Taunt(int duration) : base((int)EffectIndex.Taunt)
    {
        if (duration == 1)
        {
            Description = $"Taunts the target for {duration} turn";
        }
        else
        {
            Description = $"Taunts the target for {duration} turns";
        }
        base.duration = duration;
    }
    public override void ApplyEffect(Character attacker, Character target, SkillcheckSystem.CombatOutcome outcome)
    {
        base.ApplyEffect(attacker, target, outcome);
        target.isTaunting = true;
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
        target.isTaunting = false;
    }
}

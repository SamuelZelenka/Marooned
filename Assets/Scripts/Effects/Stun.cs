public class Stun : TickEffect
{
    public Stun(int duration, bool useOnHostile, bool useOnFriendly) : base((int)EffectIndex.Stun, useOnHostile, useOnFriendly)
    {
        base.duration = duration;
    }
    public override void ApplyEffect(Character attacker, Character target, SkillcheckSystem.CombatOutcome outcome, bool hostile)
    {
        if (IsValidEffectTarget(hostile))
        {
            base.ApplyEffect(attacker, target, outcome, hostile);
            target.isStunned = true;
        }
    }
    public override void EffectTick(Character owner)
    {
        base.EffectTick(owner);
    }
    public override void RemoveEffect(Character owner)
    {
        base.RemoveEffect(owner);
        foreach (Effect effect in owner.characterData.activeEffects)
        {
            if (effect.GetType() == this.GetType()) //If there is another stun on the owner
            {
                return;
            }
        }
        owner.isStunned = false;
    }
    public override string GetDescription()
    {
        if (duration == 1)
        {
            return $"Stunned for {duration - counter + 1} turn";
        }
        return $"Stunned for {duration - counter + 1} turns";
    }
}
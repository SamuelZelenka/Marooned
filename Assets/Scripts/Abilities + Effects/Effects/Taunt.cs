public class Taunt : TickEffect
{
    Character tauntedBy;
    public Taunt(int duration, bool useOnHostile = true, bool useOnFriendly = false) : base((int)EffectIndex.Taunt, useOnHostile, useOnFriendly, duration)
    {
    }
    public override void ApplyEffect(Character attacker, Character target, SkillcheckSystem.CombatOutcome outcome, bool hostile)
    {
        if (IsValidEffectTarget(hostile))
        {
            base.ApplyEffect(attacker, target, outcome, hostile);
            tauntedBy = attacker;
            target.tauntedBy.Add(attacker);
        }
    }
    public override void RemoveEffect(Character owner)
    {
        base.RemoveEffect(owner);
        for (int i = 0; i < owner.tauntedBy.Count; i++)
        {
            if (owner.tauntedBy[i] == tauntedBy) 
            {
                owner.tauntedBy.RemoveAt(i); //Remove the character from the list of characters taunting the owner
                break;
            }
        }
    }
    public override string GetDescription()
    {
        if (duration == 1)
        {
            return $"Taunted for {duration} turn";
        }
        return $"Taunted for {duration} turns";
    }
}

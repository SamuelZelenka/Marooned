public class StatBuff : TickEffect
{
    int buff;
    CharacterStatType statType;
    public StatBuff(int buff, CharacterStatType statType, int duration, bool useOnHostile, bool useOnFriendly) : base((int)EffectIndex.StatBuff, useOnHostile, useOnFriendly, duration)
    {
        this.buff = buff;
        this.statType = statType;
    }

    public override void ApplyEffect(Character attacker, Character target, SkillcheckSystem.CombatOutcome outcome, bool hostile)
    {
        if (IsValidEffectTarget(hostile))
        {
            base.ApplyEffect(attacker, target, outcome, hostile);
            CharacterData.Stat stat = target.characterData.GetStat(statType);
            stat.IncreaseStat(buff);
        }
    }
    public override void RemoveEffect(Character owner)
    {
        base.RemoveEffect(owner);
        CharacterData.Stat stat = owner.characterData.GetStat(statType);
        stat.DecreaseStat(buff);
    }
    public override string GetDescription()
    {
        return $"{statType.ToString()} increased by {buff}";
    }
}

public class StatDebuff : TickEffect
{
    int debuff;
    CharacterStatType statType;
    public StatDebuff(int debuff, CharacterStatType statType, int duration, bool useOnHostile, bool useOnFriendly) : base((int)EffectIndex.StatBuff, useOnHostile, useOnFriendly, duration)
    {
        this.debuff = debuff;
        this.statType = statType;
    }

    public override void ApplyEffect(Character attacker, Character target, SkillcheckSystem.CombatOutcome outcome, bool hostile)
    {
        if (IsValidEffectTarget(hostile))
        {
            base.ApplyEffect(attacker, target, outcome, hostile);
            CharacterData.Stat stat = target.characterData.GetStat(statType);
            stat.DecreaseStat(debuff);
        }
    }
    public override void RemoveEffect(Character owner)
    {
        base.RemoveEffect(owner);
        CharacterData.Stat stat = owner.characterData.GetStat(statType);
        stat.IncreaseStat(debuff);
    }
    public override string GetDescription()
    {
        return $"{statType.ToString()} decreased by {debuff}";
    }
}

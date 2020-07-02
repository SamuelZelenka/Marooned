using UnityEngine;

public abstract class Effect
{
    public const float GRACEMODIFIER = 0.5f;
    public const float CRITMODIFIER = 1.5f;

    string description;
    public Sprite effectSprite;
    public string Description
    {
        get;
        protected set;
    }
    public abstract void ApplyEffect(Character target, SkillcheckSystem.CombatOutcome outcome);

    public static int GetModifiedValue(SkillcheckSystem.CombatOutcome outcome, int originalValue)
    {
        switch (outcome)
        {
            case SkillcheckSystem.CombatOutcome.Miss:
                return 0;
            case SkillcheckSystem.CombatOutcome.Grace:
                return Mathf.RoundToInt(originalValue * GRACEMODIFIER);
            case SkillcheckSystem.CombatOutcome.NormalHit:
                return originalValue;
            case SkillcheckSystem.CombatOutcome.Critical:
                return Mathf.RoundToInt(originalValue * CRITMODIFIER);
            default:
                Debug.Log("Missing outcome type");
                return int.MinValue;
        }
    }
}

public abstract class TickEffect : Effect
{
    public override void ApplyEffect(Character target, SkillcheckSystem.CombatOutcome outcome)
    {
        target.characterData.AddEffect(this);
    }
    public virtual void EffectTick(Character target)
    {
        if (counter >= duration)
        {
            RemoveEffect(target);
        }
        counter++;
    }
    public virtual void RemoveEffect(Character target)
    {
        target.characterData.RemoveEffects(this);
    }
    protected int duration;
    private int counter = 0;
}
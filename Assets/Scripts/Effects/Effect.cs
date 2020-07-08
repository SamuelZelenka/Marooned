using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    public const float GRACEMODIFIER = 0.5f;
    public const float CRITMODIFIER = 1.5f;

    public enum EffectIndex { Bleed, Damage, Displace, LoyaltyDecrease, LoyaltyIncrease, Poison, Stun, Taunt }

    string description;
    public Sprite EffectSprite
    {
        private set;
        get;
    }
    public string Description
    {
        get;
        protected set;
    }

    const string path = "EffectSprites/";
    protected Effect(int effectIndex)
    {
        EffectSprite = Resources.Load<Sprite>(path + "EffectIcon" + effectIndex);
    }

    public abstract void ApplyEffect(Character attacker, Character target, SkillcheckSystem.CombatOutcome outcome);

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
    public abstract string GetDescription();
}

public abstract class TickEffect : Effect
{
    public TickEffect(int effectIndex) : base(effectIndex) { }

    public override void ApplyEffect(Character attacker, Character target, SkillcheckSystem.CombatOutcome outcome)
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
        target.overHeadUI.UpdateUI();
    }
    public virtual void RemoveEffect(Character target)
    {
        target.characterData.RemoveEffects(this);
    }
    protected int duration;
    protected int counter = 0;
}
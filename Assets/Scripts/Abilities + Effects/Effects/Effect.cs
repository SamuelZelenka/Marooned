using UnityEngine;

public enum Condition { Stunned, Bleeding, Poisoned}

public abstract class Effect
{
    public const float GRACEMODIFIER = 0.5f;
    public const float CRITMODIFIER = 1.5f;

    public enum EffectIndex { Bleed, Damage, Displace, Heal, LoyaltyDecrease, LoyaltyIncrease, Poison, StatBuff, StatDebuff, Stun, Taunt }

    public Sprite EffectSprite
    {
        private set;
        get;
    }

    protected bool useOnHostile, useOnFriendly;

    const string path = "EffectSprites/";
    protected Effect(int effectIndex, bool useOnHostile, bool useOnFriendly)
    {
        EffectSprite = Resources.Load<Sprite>(path + "EffectIcon" + effectIndex);
        this.useOnHostile = useOnHostile;
        this.useOnFriendly = useOnFriendly;
    }

    public abstract void ApplyEffect(Character attacker, Character target, bool crit, bool hostile);

    public static int GetModifiedValue(bool crit, int originalValue)
    {

        return crit ? Mathf.RoundToInt(originalValue * CRITMODIFIER) : originalValue;
       
    }
    public abstract string GetDescription();

    public bool IsValidEffectTarget(bool characterIsHostile)
    {
        if (characterIsHostile == useOnHostile)
        {
            return true;
        }
        else if (!characterIsHostile && useOnFriendly)
        {
            return true;
        }
        return false;
    }
}

public abstract class TickEffect : Effect
{
    public TickEffect(int effectIndex, bool useOnHostile, bool useOnFriendly, int duration) : base(effectIndex, useOnHostile, useOnFriendly)
    {
        this.duration = duration;
    }

    public override void ApplyEffect(Character attacker, Character target, bool crit, bool hostile)
    {
        if (IsValidEffectTarget(hostile))
        {
            target.characterData.AddEffect(this);
        }
    }
    public virtual void EffectTick(Character owner)
    {
        if (counter >= duration)
        {
            RemoveEffect(owner);
        }
        counter++;
        owner.overHeadUI.UpdateValues(owner);
    }
    public virtual void RemoveEffect(Character owner)
    {
        owner.characterData.RemoveEffects(this);
    }
    protected int duration;
    protected int counter = 0;
}
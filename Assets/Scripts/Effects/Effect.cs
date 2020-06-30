using UnityEngine;

public abstract class Effect
{
    string description;
    public Sprite effectSprite;
    public string Description
    {
        get;
        protected set;
    }
    public abstract void ApplyEffect(Character target);
}

public abstract class TickEffect : Effect
{
    public override void ApplyEffect(Character target)
    {
        target.AddEffect(this);
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
        target.RemoveEffects(this);
    }
    protected int duration;
    private int counter = 0;
}
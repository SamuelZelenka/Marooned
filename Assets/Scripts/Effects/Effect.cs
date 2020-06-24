public abstract class Effect
{
    string description;
    public string Description
    {
        get;
        protected set;
    }
    public abstract void ApplyEffect(Character character);
}

public abstract class TickEffect : Effect
{
    public override void ApplyEffect(Character character)
    {
        character.AddEffect(this);
    }
    public virtual void EffectTick(Character character)
    {
        if (counter >= duration)
        {
            RemoveEffect(character);
        }
        counter++;
    }
    public virtual void RemoveEffect(Character character)
    {
        character.RemoveEffects(this);
    }
    protected int duration;
    private int counter = 0;
}
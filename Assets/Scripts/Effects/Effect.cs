public abstract class Effect
{
    protected string description;

    public abstract void ApplyEffect(Character character);
    public abstract void EffectTick(Character character);
}
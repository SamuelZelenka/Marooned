public class Stun : TickEffect
{
    public Stun(int duration, bool useOnHostile, bool useOnFriendly) : base((int)EffectIndex.Stun, useOnHostile, useOnFriendly, duration)
    {
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
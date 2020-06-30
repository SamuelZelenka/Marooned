public class ChainWhip : Ability
{
    public ChainWhip()
    {
        cost = 10;
        targetType = new SwipeAdjacent();
        effects.Add(new Damage(5));
        effects.Add(new LoyaltyDecrease(5));
    }
}

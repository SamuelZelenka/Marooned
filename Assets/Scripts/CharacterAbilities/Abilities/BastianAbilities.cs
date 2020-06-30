public class ChainWhip : Ability
{
    public ChainWhip()
    {
        cost = 10;
        targeting = new SwipeAdjacent();
        effects.Add(new Damage(5));
        effects.Add(new LoyaltyDecrease(5));
    }
}

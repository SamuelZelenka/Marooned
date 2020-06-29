public class ChainWhip : Ability
{
    public ChainWhip()
    {
        cost = 10;
        targetType = new SwipeAdjacent();
        effects.Add(new Damage(5));
        effects.Add(new LoyaltyDecrease(5));
    }

    public override void Use(Character character)
    {
        foreach (var item in effects)
        {
            item.ApplyEffect(character);
        }
    }
}

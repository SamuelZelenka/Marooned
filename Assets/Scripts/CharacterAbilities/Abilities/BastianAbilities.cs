public class ChainWhip : Ability
{
    int damage = 5;
    int loyaltyDecrease = 5;
    public ChainWhip()
    {
        abilityName = this.ToString();
        abilityDescription = $"Deal {damage} vitality damage and reduce {loyaltyDecrease} loyalty from up to three adjacent targets";
        abilitySprite = null;
        cost = 10;
        effects.Add(new Damage(damage));
        effects.Add(new LoyaltyDecrease(loyaltyDecrease));
        targeting = new SwipeAdjacent();
    }
}

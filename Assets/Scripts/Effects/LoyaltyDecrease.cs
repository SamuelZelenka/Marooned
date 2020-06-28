public class LoyaltyDecrease : Effect
{
    int amount;
    public LoyaltyDecrease(int amount)
    {
        Description = $"Reduces loyalty by {amount}";
        this.amount = amount;
    }

    public override void ApplyEffect(Character character)
    {
        character.characterData.Loyalty.CurrentValue -= amount;
    }
}

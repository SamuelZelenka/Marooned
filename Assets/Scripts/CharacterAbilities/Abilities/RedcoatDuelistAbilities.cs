public class Slice : Ability
{
    int damage = 5;

    public Slice()
    {
        abilityName = this.ToString();
        abilityDescription = $"Deal {damage} to an adjacent target";
        abilitySprite = null;
        cost = 10;
        effects.Add(new Damage(damage));
        targeting = new SingleTargetAdjacent();
    }
}

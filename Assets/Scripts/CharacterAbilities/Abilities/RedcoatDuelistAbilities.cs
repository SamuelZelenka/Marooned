public class Slice : Ability
{
    public Slice()
    {
        cost = 10;
        targetType = new SingleTargetAdjacent();
        effects.Add(new Damage(5));
    }
}

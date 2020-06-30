public class Slice : Ability
{
    public Slice()
    {
        cost = 10;
        targeting = new SingleTargetAdjacent();
        effects.Add(new Damage(5));
    }
}

public class DebugAdjacentAbility : Ability
{
    public DebugAdjacentAbility()
    {
        abilityName = "Debug Adjacent Ability";
        abilityDescription = "Debug aility that will apply BLEED and STUN effect on an adjacent target.";
        isInstant = false;
        cost = 10;
        effects.Add(new Bleed(5));
        effects.Add(new Stun(2));
        targetType = new SingleTargetAdjacent();
    }

    public override void Select(Character character)
    {
        base.Select(character);
    }
}

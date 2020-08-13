public class Displace : Effect
{
    bool pull;
    int hexes;

    public Displace(bool pull, int hexes, bool useOnHostile, bool useOnFriendly) : base((int)EffectIndex.Displace, useOnHostile, useOnFriendly)
    {
        this.pull = pull;
        this.hexes = hexes;
    }

    public override void ApplyEffect(Character attacker, Character target, bool crit, bool hostile)
    {
        if (IsValidEffectTarget(hostile))
        {
            HexDirection directionToTarget = HexDirectionExtension.GetDirectionTo(attacker.Location, target.Location);

            HexCell newCell = target.Location;
            for (int i = 0; i < hexes; i++)
            {
                HexCell cellToTry = pull ? newCell.GetNeighbor(HexDirectionExtension.Opposite(directionToTarget)) : newCell.GetNeighbor(directionToTarget);
                if (!target.CanEnter(cellToTry))
                {
                    break;
                }
                newCell = cellToTry;
            }
            target.Location = newCell;
        }
    }
    public override string GetDescription()
    {
        return $"Displaced {hexes} hex(es)";
    }
}

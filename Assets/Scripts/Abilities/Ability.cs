using System.Collections.Generic;

public abstract class Ability
{
    public string abilityName;
    public string abilityDescription;
    public bool isInstant;

    protected int cost;
    protected List<Effect> effects;

    public TargetType targetType;

    public virtual void Select(Character character)
    {

    }

    public virtual void Use(Character character)
    {
        character.resources.Energy -= cost;
    }
  
    public virtual void DeSelect()
    {

    }
}
public abstract class TargetType
{
    public abstract List<HexCell> GetValidCells(HexCell fromCell);
}

public class SingleTargetAdjacent : TargetType
{
    public override List<HexCell> GetValidCells(HexCell fromCell)
    {
        List<HexCell> validCells = new List<HexCell>();

        AddCell(new HexCoordinates(0, 1));
        AddCell(new HexCoordinates(1, 0));
        AddCell(new HexCoordinates(1, -1));
        AddCell(new HexCoordinates(0, -1));
        AddCell(new HexCoordinates(-1, 0));
        AddCell(new HexCoordinates(-1, 1));

        return new List<HexCell>();

        void AddCell(HexCoordinates newCords)
        {
            if (fromCell.myGrid.GetCell(new HexCoordinates(fromCell.coordinates.X + newCords.X, fromCell.coordinates.Y + newCords.Y)) != null)
            {
                validCells.Add(fromCell.myGrid.GetCell(new HexCoordinates(fromCell.coordinates.X + newCords.X, fromCell.coordinates.Y + newCords.Y)));
            }
        }
    }
}

public class SwipeAttack : TargetType
{
    public override List<HexCell> GetValidCells(HexCell fromCell)
    {
        throw new System.NotImplementedException();
    }
}

public class NearbyAttack : TargetType
{
    public override List<HexCell> GetValidCells(HexCell fromCell)
    {
        throw new System.NotImplementedException();
    }
}

public class SingleTargetRanged : TargetType
{
    public override List<HexCell> GetValidCells(HexCell fromCell)
    {
        throw new System.NotImplementedException();
    }
}
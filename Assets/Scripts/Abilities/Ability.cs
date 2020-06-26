using System.Collections.Generic;
using UnityEngine;
public abstract class Ability
{
    public string abilityName;
    public string abilityDescription;
    public bool isInstant;

    protected int cost;
    protected List<Effect> effects = new List<Effect>();
    public TargetType targetType;

    public virtual void Select(Character character)
    {

    }

    public virtual void Use(Character character)
    {
        character.characterData.Energy.CurrentValue -= cost;
    }
  
    public virtual void DeSelect()
    {

    }
}
public abstract class TargetType
{
    public abstract List<HexCell> GetValidCells(HexCell fromCell);
    public abstract List<HexCell> GetAffectedCells(HexCell selectedCell);
}

public class SingleTargetAdjacent : TargetType
{
    public override List<HexCell> GetValidCells(HexCell fromCell)
    {
       return CellSearch.GetAdjacent(fromCell);
    }
    public override List<HexCell> GetAffectedCells(HexCell selectedCell)
    {
        List<HexCell> affectedCells = new List<HexCell>();
        affectedCells.Add(selectedCell);
        return affectedCells;
    }
}

public class SwipeAttack : TargetType
{
    HexCell unitCell;
    public override List<HexCell> GetValidCells(HexCell fromCell)
    {
        unitCell = fromCell;
        return CellSearch.GetAdjacent(fromCell);
    }
    public override List<HexCell> GetAffectedCells(HexCell selectedCell)
    {
        List<HexCell> affectedCells = new List<HexCell>();
        
        foreach (HexCell cell in CellSearch.GetShared(unitCell, selectedCell))
        {
            affectedCells.Add(selectedCell);
        }

        return affectedCells;
    }
}

public class NearbyAttack : TargetType
{
    public override List<HexCell> GetValidCells(HexCell fromCell)
    {
        throw new System.NotImplementedException();
    }
    public override List<HexCell> GetAffectedCells(HexCell fromCell)
    {
        return null;
    }
}

public class SingleTargetRanged : TargetType
{
    public override List<HexCell> GetValidCells(HexCell fromCell)
    {
        throw new System.NotImplementedException();
    }
    public override List<HexCell> GetAffectedCells(HexCell fromCell)
    {
        return null;
    }
}

public static class CellSearch
{
    public static List<HexCell> GetAdjacent(HexCell fromCell)
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

    public static List<HexCell> GetShared(HexCell firstCell, HexCell secondCell)
    {

        List<HexCell> validCells = new List<HexCell>();

        foreach (HexCell first in GetAdjacent(firstCell))
        {
            foreach (HexCell second in GetAdjacent(secondCell))
            {
                if (first == second)
                {
                    validCells.Add(first);
                }
            }
        }
        return validCells;
    }
}
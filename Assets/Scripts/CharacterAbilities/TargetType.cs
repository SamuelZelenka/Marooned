using System.Collections.Generic;

public abstract class TargetType
{
    public abstract List<HexCell> GetValidTargets(HexCell fromCell);
    public abstract List<HexCell> GetAffectedCells(HexCell fromCell, HexCell targetCell);
    public void GetAffectedCharacters(Character abilityUser, HexCell fromCell, HexCell targetCell, out List<Character> enemies, out List<Character> friendlies)
    {
        enemies = new List<Character>();
        friendlies = new List<Character>();

        foreach (var cell in GetAffectedCells(fromCell, targetCell))
        {
            Character character = cell.Unit as Character;
            if (character)
            {
                if (character.isFriendlyTo(abilityUser))
                {
                    friendlies.Add(character);
                }
                else
                {
                    enemies.Add(character);
                }
            }
        }
    }
}

public class SingleTargetAdjacent : TargetType
{
    bool withUnit;
    public SingleTargetAdjacent(bool withUnit = true)
    {
        this.withUnit = withUnit;
    }
    public override List<HexCell> GetValidTargets(HexCell fromCell)
    {
        return CellFinder.GetAllAdjacent(fromCell, true, !withUnit, withUnit);
    }
    public override List<HexCell> GetAffectedCells(HexCell fromCell, HexCell targetCell)
    {
        List<HexCell> affectedCells = new List<HexCell>();
        affectedCells.Add(targetCell);
        return affectedCells;
    }
}

public class SwipeAdjacent : TargetType
{
    public override List<HexCell> GetValidTargets(HexCell fromCell)
    {
        return CellFinder.GetAllAdjacent(fromCell, true, false, false);
    }
    public override List<HexCell> GetAffectedCells(HexCell fromCell, HexCell targetCell)
    {
        List<HexCell> affectedCells = new List<HexCell>();
        affectedCells.Add(targetCell);

        //Sides
        HexDirection dirToSelected = HexDirectionExtension.GetDirectionToNeighbor(fromCell, targetCell);
        HexCell previousCell = fromCell.GetNeighbor(dirToSelected.Previous(), true, false, false, false, false);
        if (previousCell)
        {
            affectedCells.Add(previousCell);
        }
        HexCell nextCell = fromCell.GetNeighbor(dirToSelected.Next(), true, false, false, false, false);
        if (nextCell)
        {
            affectedCells.Add(nextCell);
        }
        return affectedCells;
    }
}

public class AnySingleTarget : TargetType
{
    public override List<HexCell> GetValidTargets(HexCell fromCell)
    {
        return CellFinder.GetAllCells(fromCell.myGrid, true, true);
    }

    public override List<HexCell> GetAffectedCells(HexCell fromCell, HexCell targetCell)
    {
        List<HexCell> affectedCells = new List<HexCell>();
        affectedCells.Add(targetCell);
        return affectedCells;
    }
}

public class SingleTargetRanged : TargetType
{
    int range;
    public SingleTargetRanged(int range)
    {
        this.range = range;
    }

    public override List<HexCell> GetValidTargets(HexCell fromCell)
    {
        return CellFinder.GetCellsWithinRange(fromCell, range, true, true);
    }

    public override List<HexCell> GetAffectedCells(HexCell fromCell, HexCell targetCell)
    {
        List<HexCell> affectedCells = new List<HexCell>();
        affectedCells.Add(targetCell);
        return affectedCells;
    }
}

public class SingleTargetRangeLine : TargetType
{
    int range;
    public SingleTargetRangeLine(int range)
    {
        this.range = range;
    }

    public override List<HexCell> GetValidTargets(HexCell fromCell)
    {
        return CellFinder.GetInLine(fromCell, true, true, range, 0);
    }

    public override List<HexCell> GetAffectedCells(HexCell fromCell, HexCell targetCell)
    {
        List<HexCell> affectedCells = new List<HexCell>();
        affectedCells.Add(targetCell);
        return affectedCells;
    }
}

public class CollateralRangeLine : TargetType
{
    int range;
    int rangeAfterFirstHit;
    public CollateralRangeLine(int range, int rangeAfterFirstHit)
    {
        this.range = range;
        this.rangeAfterFirstHit = rangeAfterFirstHit;
    }

    public override List<HexCell> GetValidTargets(HexCell fromCell)
    {
        return CellFinder.GetInLine(fromCell, true, true, range, 0);
    }

    public override List<HexCell> GetAffectedCells(HexCell fromCell, HexCell targetCell)
    {
        List<HexCell> affectedCells = new List<HexCell>();
        affectedCells.AddRange(CellFinder.GetInLine(fromCell, true, true, range, rangeAfterFirstHit));
        return affectedCells;
    }
}

public class AOE : TargetType
{
    int range;
    int aoeRange;
    bool selfOriginating = false;

    public AOE(int range, int aoeRange, bool selfOriginating)
    {
        this.range = range;
        this.aoeRange = aoeRange;
        this.selfOriginating = selfOriginating;
    }

    public override List<HexCell> GetValidTargets(HexCell fromCell)
    {
        List<HexCell> validCells = new List<HexCell>();
        if (selfOriginating)
        {
            validCells.Add(fromCell);
        }
        else
        {
            validCells.AddRange(CellFinder.GetCellsWithinRange(fromCell, range, true, true));
        }
        return validCells;
    }

    public override List<HexCell> GetAffectedCells(HexCell fromCell, HexCell targetCell)
    {
        List<HexCell> affectedCells = new List<HexCell>();
        affectedCells.AddRange(CellFinder.GetCellsWithinRange(targetCell, aoeRange, true, false));
        affectedCells.Add(targetCell);
        return affectedCells;
    }
}
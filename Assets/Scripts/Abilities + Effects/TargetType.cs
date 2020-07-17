using System.Collections.Generic;

public abstract class TargetType
{
    public abstract List<HexCell> GetValidTargetCells(HexCell fromCell, bool interactable);
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
    public override List<HexCell> GetValidTargetCells(HexCell fromCell, bool interactable)
    {
        return CellFinder.GetAllAdjacentCells(fromCell, (c) => c.Traversable == true);
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
    public override List<HexCell> GetValidTargetCells(HexCell fromCell, bool interactable)
    {
        return CellFinder.GetAllAdjacentCells(fromCell, (c) => c.Traversable == true);
    }
    public override List<HexCell> GetAffectedCells(HexCell fromCell, HexCell targetCell)
    {
        List<HexCell> affectedCells = new List<HexCell>();
        affectedCells.Add(targetCell);

        //Sides
        HexDirection dirToSelected = HexDirectionExtension.GetDirectionToNeighbor(fromCell, targetCell);
        HexCell previousCell = Utility.TestVariableAgainstConditions(fromCell.GetNeighbor(dirToSelected.Previous()), (c) => c.Traversable == true);
        if (previousCell)
        {
            affectedCells.Add(previousCell);
        }
        HexCell nextCell = Utility.TestVariableAgainstConditions(fromCell.GetNeighbor(dirToSelected.Next()), (c) => c.Traversable == true);
        if (nextCell)
        {
            affectedCells.Add(nextCell);
        }
        return affectedCells;
    }
}

public class AnySingleTarget : TargetType
{
    public override List<HexCell> GetValidTargetCells(HexCell fromCell, bool interactable)
    {
        return CellFinder.GetAllCells(fromCell.myGrid, (c) => c.Traversable == true);
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

    public override List<HexCell> GetValidTargetCells(HexCell fromCell, bool interactable)
    {
        return CellFinder.GetCellsWithinRange(fromCell, range, (c) => c.Traversable == true);
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
    bool targetBlockedByFirstUnit;
    public SingleTargetRangeLine(int range, bool targetBlockedByFirstUnit)
    {
        this.range = range;
        this.targetBlockedByFirstUnit = targetBlockedByFirstUnit;
    }

    public override List<HexCell> GetValidTargetCells(HexCell fromCell, bool interactable)
    {
        if (interactable && targetBlockedByFirstUnit)
        {
            return CellFinder.GetFirstCellInAllLines(fromCell, range, (c) => c.Traversable == true, (c) => c.Unit != null);
        }
        else
        {
            return CellFinder.GetFirstCellInAllLines(fromCell, range, (c) => c.Traversable == true);
        }
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
    bool targetBlockedByFirstUnit;
    public CollateralRangeLine(int range, int rangeAfterFirstHit, bool targetBlockedByFirstUnit)
    {
        this.range = range;
        this.rangeAfterFirstHit = rangeAfterFirstHit;
        this.targetBlockedByFirstUnit = targetBlockedByFirstUnit;
    }

    public override List<HexCell> GetValidTargetCells(HexCell fromCell, bool interactable)
    {
        if (interactable && targetBlockedByFirstUnit)
        {
            return CellFinder.GetFirstCellInAllLines(fromCell, range, (c) => c.Traversable == true, (c) => c.Unit != null);
        }
        else
        {
            return CellFinder.GetFirstCellInAllLines(fromCell, range, (c) => c.Traversable == true);
        }
    }

    public override List<HexCell> GetAffectedCells(HexCell fromCell, HexCell targetCell)
    {
        List<HexCell> affectedCells = new List<HexCell>();
        HexDirection directionToTarget = HexDirectionExtension.GetDirectionTo(fromCell, targetCell);
        affectedCells.Add(targetCell);
        affectedCells.AddRange(CellFinder.GetAllCellsInLine(fromCell, directionToTarget, rangeAfterFirstHit, (c) => c.Traversable == true));
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

    public override List<HexCell> GetValidTargetCells(HexCell fromCell, bool interactable)
    {
        List<HexCell> validCells = new List<HexCell>();
        if (selfOriginating)
        {
            validCells.Add(fromCell);
        }
        else
        {
            validCells.AddRange(CellFinder.GetCellsWithinRange(fromCell, range, (c) => c.Traversable == true));
        }
        return validCells;
    }

    public override List<HexCell> GetAffectedCells(HexCell fromCell, HexCell targetCell)
    {
        List<HexCell> affectedCells = new List<HexCell>();
        affectedCells.AddRange(CellFinder.GetCellsWithinRange(targetCell, aoeRange, (c) => c.Traversable == true));
        affectedCells.Add(targetCell);
        return affectedCells;
    }
}
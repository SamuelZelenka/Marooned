using System.Collections.Generic;
using System;

public static class CellFinder
{
    public static List<HexCell> GetAllAdjacent(HexCell fromCell, bool traversable, bool freeRequirement, bool unitRequirement)
    {
        List<HexCell> validCells = new List<HexCell>();
        validCells.AddRange(fromCell.GetNeighbors(traversable, freeRequirement, false, false, unitRequirement));
        return validCells;
    }

    //NEW IS ALWAYS BETTER : TODO EXCHANGE OLD FOR THIS
    public static List<HexCell> GetCellsWithCondition(HexGrid hexGrid, params Func<HexCell, bool>[] conditions)
    {
        return hexGrid.GetAllCellsWithCondition(conditions);
    }

    public static List<HexCell> GetAllCells(HexGrid hexGrid, bool traversable, bool hasUnit)
    {
        return hexGrid.GetAllCellsWithCondition((c) => c.Traversable == traversable, (c) => c.Unit == hasUnit);
    }

    public static List<HexCell> GetCellsInAllLines(HexCell fromCell, bool traversable, bool hasUnit, int range, int rangeAfterFirstHit)
    {
        List<HexCell> validCells = new List<HexCell>();

        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            validCells.AddRange(GetCellsInDirection(fromCell, d, traversable, hasUnit, range, rangeAfterFirstHit));
        }
        return validCells;
    }

    public static List<HexCell> GetCellsInDirection(HexCell fromCell, HexDirection direction, bool traversable, bool hasUnit, int range, int rangeAfterFirstHit)
    {
        List<HexCell> validCells = new List<HexCell>();

        HexCell cellToTest = fromCell;
        for (int i = 0; i < range; i++)
        {
            cellToTest = cellToTest.GetNeighbor(direction);
            if (!cellToTest)
            {
                break;
            }
            if (traversable && !cellToTest.Traversable)
            {
                continue;
            }
            if (hasUnit && !cellToTest.Unit)
            {
                continue;
            }
            validCells.Add(cellToTest);
            for (int j = 0; j < rangeAfterFirstHit; j++)
            {
                cellToTest = cellToTest.GetNeighbor(direction);
                if (!cellToTest)
                {
                    break;
                }
                if (traversable && !cellToTest.Traversable)
                {
                    continue;
                }
                if (hasUnit && !cellToTest.Unit)
                {
                    continue;
                }
                validCells.Add(cellToTest);
            }
            break;
        }
        return validCells;
    }

    public static List<HexCell> GetCellsWithinRange(HexCell fromCell, int range, bool traversable, bool hasUnit)
    {
        List<HexCell> cellsWithinReach = Pathfinding.GetCellsWithinRange(fromCell, range, traversable);
        if (hasUnit)
        {
            for (int i = 0; i < cellsWithinReach.Count; i++)
            {
                if (cellsWithinReach[i].Unit == null)
                {
                    cellsWithinReach.RemoveAt(i);
                    i--;
                }
            }
        }
        return cellsWithinReach;
    }
}
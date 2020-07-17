using System.Collections.Generic;
using System;

public static class CellFinder
{
    public static List<HexCell> GetAllAdjacentCells(HexCell fromCell, params Func<HexCell, bool>[] conditions)
    {
        List<HexCell> validCells = new List<HexCell>();
        validCells.PopulateListWithMatchingConditions(fromCell.Neighbors, conditions);
        return validCells;
    }

    public static List<HexCell> GetAllCells(HexGrid hexGrid, params Func<HexCell, bool>[] conditions)
    {
        return hexGrid.GetAllCellsWithCondition(conditions);
    }

    public static List<HexCell> GetFirstCellInAllLines(HexCell fromCell, int range, params Func<HexCell, bool>[] conditions)
    {
        List<HexCell> validCells = new List<HexCell>();
        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            HexCell newCell = GetFirstCellInLine(fromCell, d, range, conditions);
            if (newCell)
            {
                validCells.Add(newCell);
            }
        }
        return validCells;
    }

    public static HexCell GetFirstCellInLine(HexCell fromCell, HexDirection direction, int range, params Func<HexCell, bool>[] conditions)
    {
        HexCell cellToTest = fromCell;
        for (int i = 0; i < range; i++)
        {
            cellToTest = cellToTest.GetNeighbor(direction);
            if (!cellToTest)
            {
                break;
            }
            
            if (Utility.TestVariableAgainstConditions(cellToTest, conditions) == null)
            {
                continue;
            }
            return cellToTest;
        }
        return null;
    }

    public static List<HexCell> GetAllCellsInLine(HexCell fromCell, HexDirection direction, int range, params Func<HexCell, bool>[] conditions)
    {
        List<HexCell> foundCells = new List<HexCell>();
        HexCell cellToTest = fromCell;
        for (int i = 0; i < range; i++)
        {
            cellToTest = cellToTest.GetNeighbor(direction);
            if (!cellToTest)
            {
                break;
            }
            cellToTest = Utility.TestVariableAgainstConditions(cellToTest, conditions);
            if (!cellToTest)
            {
                continue;
            }
            foundCells.Add(cellToTest);
        }
        return foundCells;
    }

    public static List<HexCell> GetCellsWithinRange(HexCell fromCell, int range, params Func<HexCell, bool>[] conditions)
    {
        List<HexCell> cellsWithinReach = Pathfinding.GetCellsWithinRange(fromCell, range);
        List<HexCell> cellsMathcingConditions = new List<HexCell>();
        cellsMathcingConditions.PopulateListWithMatchingConditions(cellsWithinReach, conditions);
        return cellsMathcingConditions;
    }
}
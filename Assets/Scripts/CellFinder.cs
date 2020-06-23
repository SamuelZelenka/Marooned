using System;
using System.Collections.Generic;
using UnityEngine;

public static class CellFinder
{

    public enum Direction { NE, E, SE, SW, W, NW }
    static List<HexCell> validCells = new List<HexCell>();

    static Dictionary<Direction, HexCoordinates> directionDictionary = new Dictionary<Direction, HexCoordinates>
    {
        {Direction.NE, new HexCoordinates(0, 1)},
        {Direction.E, new HexCoordinates(1, 0)},
        {Direction.SE, new HexCoordinates(1, -1)},
        {Direction.SW, new HexCoordinates(0, -1)},
        {Direction.W, new HexCoordinates(-1, 0)},
        {Direction.NW, new HexCoordinates(-1, 1)}
    };
    public static List<HexCell> GetAllAdjacent(HexCell cell)
    {
        validCells.Clear();
        foreach (Direction dir in Enum.GetValues(typeof(Direction)))
        {
            AddCell(directionDictionary[dir], cell);
        }
        return validCells;
    }
    public static List<HexCell> GetLine(HexCell fromCell, Direction direction, int range)
    {
        validCells.Clear();
        for (int i = 0; i < range; i++)
        {
            AddCell(new HexCoordinates(directionDictionary[direction].X * i, directionDictionary[direction].Y * i), fromCell);
        }
        return validCells;
    }
    public static List<HexCell> GetFan(HexCell fromCell, Direction direction, bool isLarge)
    {
        const int LARGESIZE = 2, SMALLSIZE = 1;
        int size = SMALLSIZE;
        validCells.Clear();
        if (isLarge)
        {
            size = LARGESIZE;
        }
        for (int i = -1 * size; i < size; i++)
        {
            Direction dir = (Direction)Enum.GetValues(direction.GetType()).GetValue(i);
            AddCell(new HexCoordinates(directionDictionary[dir].X, directionDictionary[dir].Y), fromCell);
        }
        return validCells;
    }

    static void AddCell(HexCoordinates newCords, HexCell cell)
    {
        if (cell.myGrid.GetCell(new HexCoordinates(cell.coordinates.X + newCords.X, cell.coordinates.Y + newCords.Y)) != null)
        {
            validCells.Add(cell.myGrid.GetCell(new HexCoordinates(cell.coordinates.X + newCords.X, cell.coordinates.Y + newCords.Y)));
        }
    }
}
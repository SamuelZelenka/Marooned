using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route
{
    const int SPAWNRANGE = 2;

    public HexCell[] RouteStops
    {
        get;
        private set;
    }

    public Route(HexCell[] stops)
    {
        RouteStops = stops;
    }

    public HexCell GetSpawnableLocation()
    {
        List<HexCell> cellsToTest = new List<HexCell>();
        foreach (HexCell stop in RouteStops)
        {
            cellsToTest.AddRange(Pathfinding.GetCellsWithinRange(stop, SPAWNRANGE, (c) => c.Traversable == true, (c) => c.IsOcean == true, (c) => c.IsFree == true));
        }
        while (cellsToTest.Count > 0)
        {
            HexCell cell = Utility.ReturnRandom(cellsToTest);
            if (cell != null && cell.Unit == null && cell.Traversable)
            {
                return cell;
            }
            cellsToTest.Remove(cell);
        }
        Debug.LogWarning("Could not find a free cell of the requested spawntype");
        return null;
    }
}

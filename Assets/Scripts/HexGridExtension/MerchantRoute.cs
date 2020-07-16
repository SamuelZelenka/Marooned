using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantRoute
{
    const int SPAWNRANGE = 5;

    public HexCell[] RouteStops
    {
        get;
        private set;
    }

    public MerchantRoute(HexCell[] harborCells)
    {
        RouteStops = harborCells;
    }

    public HexCell GetSpawnableLocation()
    {
        List<HexCell> cellsToTest = new List<HexCell>();
        foreach (HexCell harbor in RouteStops)
        {
            cellsToTest.AddRange(Pathfinding.GetCellsWithinRange(harbor, SPAWNRANGE, true));
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

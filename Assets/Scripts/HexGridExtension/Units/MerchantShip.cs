using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantShip : Ship
{
    Route route;
    int routeIndex = 0;

    public void Setup(Route route)
    {
        this.route = route;
    }

    HexCell target;
    public override IEnumerator PerformAutomaticTurn()
    {
        if (target)
        {
            //Debug.Log("Moving from" + Location.coordinates + " to " + target.coordinates);
            yield return MoveToTarget();
        }
        else
        {
            target = FindTarget();
            //Debug.Log("AI Finding path from " + Location.coordinates.ToString() + " to " + target.coordinates.ToString());
            yield return MoveToTarget();
        }
    }

    const int maxSearchRange = 4;
    IEnumerator MoveToTarget()
    {
        Pathfinding.FindPath(Location, target, this, playerControlled);
        int searchRange = 1;
        while (!Pathfinding.HasPath && searchRange <= maxSearchRange)
        {
            Debug.Log("Checking alternative path with merchant");
            List<HexCell> alternativeTargetCells = CellFinder.GetCellsWithinRange(target, searchRange, (c) => c.Traversable == true, (c) => c.IsFree);
            foreach (var item in alternativeTargetCells)
            {
                Pathfinding.FindPath(Location, item, this, playerControlled);
                if (Pathfinding.HasPath)
                {
                    break;
                }
            }
            searchRange *= 2;
        }
        if (Pathfinding.HasPath)
        {
            yield return Travel(Pathfinding.GetReachablePath(this, out int cost));
            Pathfinding.ClearPath();
        }
        if (Location == target)
        {
            routeIndex++;
            if (routeIndex >= route.RouteStops.Length)
            {
                routeIndex = 0;
            }
            target = null;
        }
    }

    private HexCell FindTarget() => route.RouteStops[routeIndex];
}

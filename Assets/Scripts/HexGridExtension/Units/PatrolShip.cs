using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolShip : Ship
{
    Route route;
    int routeIndex = 0;
    bool chasingPlayer = false;

    public void Setup(Route route)
    {
        this.route = route;
    }

    HexCell target;
    public override IEnumerator PerformAutomaticTurn(int visionRange)
    {
        //Check for player
        CurrentVisionRange = visionRange;
        Debug.Log($"Searching for player with a vision range of {CurrentVisionRange}");
        List<HexCell> playerControlledCells = CellFinder.GetCellsWithinRange(Location, CurrentVisionRange, (c) => c.Unit != null && c.Unit.playerControlled == true);
        if (playerControlledCells.Count > 1)
        {
            Debug.LogWarning("Found multiple player controlled units to follow with patrolship");
        }
        if (playerControlledCells.Count > 0)
        {
            List<HexCell> hexesAroundPlayer = CellFinder.GetAllAdjacentCells(playerControlledCells[0], (c) => c.Traversable == true, (c) => c.IsOcean == true);
            if (hexesAroundPlayer.Count > 0)
            {
                Debug.Log("Chasing player");
                target = hexesAroundPlayer[0];
                chasingPlayer = true;
            }
            else if (chasingPlayer)
            {
                target = FindRouteTarget();
            }
        }
        else if (chasingPlayer)
        {
            target = FindRouteTarget();
        }

        //Move to found target or find new one if no one is set
        if (target)
        {
            //Debug.Log("Moving from" + Location.coordinates + " to " + target.coordinates);
            yield return MoveToTarget();
        }
        else
        {
            target = FindRouteTarget();
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
            Debug.Log("Checking alternative path with patrolship");
            List<HexCell> alternativeTargetCells = CellFinder.GetCellsWithinRange(target, searchRange, (c) => c.Traversable == true, (c) => c.IsOcean == true, (c) => c.IsFree);
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

    private HexCell FindRouteTarget() => route.RouteStops[routeIndex];
}

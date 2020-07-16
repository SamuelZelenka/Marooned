using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantShip : Ship
{
    MerchantRoute route;
    int routeIndex = 0;

    public void Setup(MerchantRoute route)
    {
        this.route = route;
    }

    HexCell target;
    public override IEnumerator PerformAutomaticTurn()
    {
        if (target)
        {
            Debug.Log("Moving from" + Location.coordinates + " to " + target.coordinates);
            yield return MoveToTarget();
        }
        else
        {
            target = FindTarget();
            Debug.Log("AI Finding path from " + Location.coordinates.ToString() + " to " + target.coordinates.ToString());
            yield return MoveToTarget();
        }
    }

    IEnumerator MoveToTarget()
    {
        Pathfinding.FindPath(Location, target, this, playerControlled);
        int tries = 0;
        while (!Pathfinding.HasPath && tries < 100) //Target unreachable
        {
            HexCell adjacentToTarget = target.GetNeighbor(HexDirectionExtension.ReturnRandomDirection());
            if (adjacentToTarget)
            {
                Pathfinding.FindPath(Location, adjacentToTarget, this, playerControlled);
            }
            tries++;
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

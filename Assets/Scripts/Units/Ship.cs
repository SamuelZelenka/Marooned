using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : HexUnit
{
    int hull = 25, maxHull = 25;
    public int Hull
    {
        set
        {
            hull = Mathf.Min(maxHull, value);
        }
        get => hull;
    }
    float cleanliness = 1;
    public float Cleanliness
    {
        get => cleanliness;
        set
        {
            cleanliness = Mathf.Min(value, 1);
        }
    }

    public override bool CanMoveTo(HexCell cell)
    {
        if (cell.Unit)
        {
            return false;
        }
        if (cell.IsLand)
        {
            if (!cell.HasHarbor)
            {
                return false;
            }
        }
        return true;
    }

    public override IEnumerator StartNewTurn()
    {
        if (playerControlled)
        {

        }
        else
        {
            remainingMovementPoints = defaultMovementPoints;
            yield return PerformAutomaticTurn();
        }
    }

    HexCell target;
    public override IEnumerator PerformAutomaticTurn()
    {
        if (target)
        {
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
        pathfinding.FindPath(Location, target, this);
        int tries = 0;
        while (!pathfinding.HasPath && tries < 100) //Target unreachable
        {
            HexCell adjacentToTarget = target.GetNeighbor(HexDirectionExtension.ReturnRandomDirection());
            if (adjacentToTarget)
            {
                pathfinding.FindPath(Location, adjacentToTarget, this);
            }
            tries++;
        }
        if (pathfinding.HasPath)
        {
            yield return Travel(pathfinding.GetReachablePath(this, out int cost));
            remainingMovementPoints -= cost;
            pathfinding.ClearPath();
        }
        if (Location == target)
        {
            target = null;
        }
    }

    private HexCell FindTarget()
    {
        HexCell newTarget = Utility.ReturnRandom(myGrid.Harbors);
        while (Location == newTarget)
        {
            newTarget = Utility.ReturnRandom(myGrid.Harbors);
        }
        return newTarget;
    }
}

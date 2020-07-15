using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : HexUnit
{
    #region Stats
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
    #endregion

    public override bool CanEnter(HexCell cell)
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

    public override void MakeUnitActive()
    {
        base.MakeUnitActive();
        if (playerControlled)
        {

        }
        else
        {
            remainingMovementPoints = defaultMovementPoints;
        }
    }

    #region AI
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
            remainingMovementPoints -= cost;
            Pathfinding.ClearPath();
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
    #endregion
}

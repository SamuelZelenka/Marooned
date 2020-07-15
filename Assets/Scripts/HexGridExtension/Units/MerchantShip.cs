using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantShip : Ship
{
    TerrainMap terrainMap;

    public void Setup(TerrainMap terrainMap)
    {
        this.terrainMap = terrainMap;
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
        HexCell newTarget = Utility.ReturnRandom(terrainMap.Harbors);
        while (Location == newTarget)
        {
            newTarget = Utility.ReturnRandom(terrainMap.Harbors);
        }
        return newTarget;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    int searchFrontierPhase = 0;
    HexCellPriorityQueue searchFrontier = null;

    HexCell currentPathFrom, currentPathTo;
    public bool HasPath
    {
        set;
        get;
    }

    public void FindPath(HexCell fromCell, HexCell toCell, HexUnit unit)
    {
        ClearPath();
        currentPathFrom = fromCell;
        currentPathTo = toCell;
        HasPath = Search(fromCell, toCell, unit);
        ShowPath(unit.maxMovement);
    }

    //Pathfinding search
    bool Search(HexCell fromCell, HexCell toCell, HexUnit unit)
    {
        searchFrontierPhase += 2;
        if (searchFrontier == null)
        {
            searchFrontier = new HexCellPriorityQueue();
        }
        else
        {
            searchFrontier.Clear();
        }

        fromCell.SearchPhase = searchFrontierPhase;
        fromCell.MovementCost = 0;
        searchFrontier.Enqueue(fromCell);

        while (searchFrontier.Count > 0)
        {
            HexCell current = searchFrontier.Dequeue();
            current.SearchPhase += 1;

            if (current == toCell)
            {
                return true;
            }

            int currentTurn = (current.MovementCost - 1) / unit.maxMovement;

            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = current.GetNeighbor(d);
                if (neighbor == null || neighbor.SearchPhase > searchFrontierPhase)
                {
                    continue;
                }

                //Hexes forbidden to move to
                if (!unit.CanMoveTo(neighbor))
                {
                    continue;
                }
                if (!neighbor.Traversable)
                {
                    continue;
                }
                //

                int hexEnterCost = 0;

                //Special conditions here

                //Default cost
                hexEnterCost += neighbor.IsOcean ? unit.oceanMovementCost : unit.landMovementCost;

                int combinedCost = current.MovementCost + hexEnterCost;
                int turn = (combinedCost - 1) / unit.maxMovement;
                if (turn > currentTurn)
                {
                    combinedCost = turn * unit.maxMovement + hexEnterCost;
                }

                if (neighbor.SearchPhase < searchFrontierPhase) //Has not been set before
                {
                    neighbor.SearchPhase = searchFrontierPhase;
                    neighbor.MovementCost = combinedCost;
                    neighbor.PathFrom = current;
                    neighbor.SearchHeuristic = neighbor.coordinates.DistanceTo(toCell.coordinates);
                    searchFrontier.Enqueue(neighbor);
                }
                else if (combinedCost < neighbor.MovementCost) //Update cost
                {
                    int oldPriority = neighbor.SearchPriority;
                    neighbor.MovementCost = combinedCost;
                    neighbor.PathFrom = current;
                    searchFrontier.Change(neighbor, oldPriority);
                }
            }
        }
        return false;
    }

    public List<HexCell> GetPath()
    {
        if (!HasPath)
        {
            return null;
        }
        List<HexCell> path = new List<HexCell>();
        for (HexCell c = currentPathTo; c != currentPathFrom; c = c.PathFrom)
        {
            path.Add(c);
        }
        path.Add(currentPathFrom);
        path.Reverse();
        return path;
    }

    void ShowPath(int maxMovement)
    {
        if (HasPath)
        {
            HexCell current = currentPathTo;
            while (current != currentPathFrom)
            {
                int turn = (current.MovementCost - 1) / maxMovement;
                current.SetLabel(turn.ToString());
                //current.SetLabel(current.MovementCost.ToString());
                current.SetHighlightStatus(true, Color.white);
                current = current.PathFrom;
            }
        }
        currentPathFrom.SetHighlightStatus(true, Color.blue);
        currentPathTo.SetHighlightStatus(true, Color.red);
    }

    public void ClearPath()
    {
        if (HasPath)
        {
            HexCell current = currentPathTo;
            while (current != currentPathFrom)
            {
                current.SetLabel(null);
                current.SetHighlightStatus(false, Color.white);
                current = current.PathFrom;
            }
            current.SetHighlightStatus(false, Color.white);
            HasPath = false;
        }
        else if (currentPathFrom)
        {
            currentPathFrom.SetHighlightStatus(false, Color.white);
            currentPathTo.SetHighlightStatus(false, Color.white);
        }
        currentPathFrom = currentPathTo = null;
    }
}

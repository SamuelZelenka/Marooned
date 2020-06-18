using System.Collections.Generic;
using UnityEngine;

public static class Pathfinding
{
    static int searchFrontierPhase = 0;
    static HexCellPriorityQueue searchFrontier = null;


    static HexUnit searcherUnit;
    static HexCell currentPathFrom, currentPathTo;
    public static bool HasPath
    {
        set;
        get;
    }

    public static void FindPath(HexCell fromCell, HexCell toCell, HexUnit unit, bool displayPath)
    {
        ClearPath();
        //New unit == Clear old pathfinding
        if (searcherUnit != unit)
        {
            unit.myGrid.ClearSearchHeuristics();
            searchFrontierPhase = 0;
            searchFrontier = null;
        }
        searcherUnit = unit;
        currentPathFrom = fromCell;
        currentPathTo = toCell;
        HasPath = Search(fromCell, toCell, unit);
        if (displayPath)
        {
            ShowPath(unit.defaultMovementPoints, unit.remainingMovementPoints);
        }
    }

    //Pathfinding search
    static bool Search(HexCell fromCell, HexCell toCell, HexUnit unit)
    {
        searchFrontierPhase += 2;
        Debug.Log("Searchfrontier phase is " + searchFrontierPhase.ToString());
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

            int currentTurn = (current.MovementCost - 1) / unit.defaultMovementPoints;

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

                //Special condition costs here
                hexEnterCost += neighbor.MovementCostPenalty;

                //Default cost
                hexEnterCost += neighbor.IsOcean ? unit.oceanMovementCost : unit.landMovementCost;

                int combinedCost = current.MovementCost + hexEnterCost;
                int turn = (combinedCost - 1) / unit.defaultMovementPoints;
                if (turn > currentTurn)
                {
                    combinedCost = turn * unit.defaultMovementPoints + hexEnterCost;
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

    public static List<HexCell> GetWholePath()
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

    public static List<HexCell> GetReachablePath(HexUnit unit, out int cost)
    {
        if (!HasPath)
        {
            cost = 0;
            return null;
        }
        List<HexCell> path = new List<HexCell>();
        for (HexCell c = currentPathTo; c != currentPathFrom; c = c.PathFrom)
        {
            if (c.MovementCost <= unit.remainingMovementPoints)
            {
                path.Add(c);
            }
        }
        path.Add(currentPathFrom);
        path.Reverse();
        cost = path[path.Count - 1].MovementCost;
        return path;
    }

    static void ShowPath(int maxMovement, int remainingMovement)
    {
        if (HasPath)
        {
            HexCell current = currentPathTo;
            while (current != currentPathFrom)
            {
                int turn = 0;
                if (current.MovementCost - remainingMovement > 0)
                {
                    turn = (current.MovementCost - 1 - remainingMovement) / maxMovement;
                    turn++;
                }
                //current.SetLabel(turn.ToString());
                current.SetLabel(current.MovementCost.ToString());
                current.SetHighlightStatus(true, Color.white);
                current = current.PathFrom;
            }
            currentPathFrom.SetHighlightStatus(true, Color.blue);
            currentPathTo.SetHighlightStatus(true, Color.red);
        }
    }

    public static void ClearPath()
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

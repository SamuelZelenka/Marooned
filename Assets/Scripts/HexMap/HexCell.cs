using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HexCell : MonoBehaviour
{
    public enum HighlightType { ActiveCell, Target, PathfindingEnd, AbilityAffected, ValidMoveInteraction, ValidCombatInteraction }

    public Text label;
    [Header("Grids")]
    public SpriteRenderer gameGrid;
    public SpriteRenderer editorGrid;
    [Header("Visual outlines and markers")]
    public SpriteRenderer activeCell;
    public SpriteRenderer target;
    public SpriteRenderer pathfindingEnd;
    public SpriteRenderer abilityAffected;
    public SpriteRenderer validMoveInteraction;
    public SpriteRenderer validCombatInteraction;

    public SpriteRenderer visualPathFrom;
    public SpriteRenderer visualPathTo;

    public HexCoordinates coordinates;
    public HexGrid myGrid;

    public HexUnit Unit { get; set; }
    bool traversable = false;
    public bool Traversable
    {
        get => traversable;
        set
        {
            traversable = value;
            ChangeEditOutlineColor(value);
        }
    }
    public HexObject Object { get; set; }
    public bool IsFree
    {
        get
        {
            if (Object != null)
            {
                return false;
            }
            if (Unit != null)
            {
                return false;
            }
            return true;
        }
    }

    public enum SpawnType { Forbidden, Player, AnyEnemy, MeleeEnemy, SupportEnemy, RangedEnemy }
    public SpawnType TypeOfSpawnPos { get; set; }

    public bool showNeighborGizmos = true;

    public delegate void HexCellHandler(HexCell cell);
    public static HexCellHandler OnHexCellHoover;

    #region Terrain and Features
    public bool IsLand { get; set; }
    public bool IsOcean { get => !IsLand; }

    public bool HasHarbor { get; set; }

    private int bitmask;
    public int Bitmask
    {
        get => bitmask;
        private set
        {
            bitmask = value;
            myGrid.SetTerrainCellVisual(this);
        }
    }

    public void CalculateBitmask()
    {
        if (IsOcean)
        {
            Bitmask = -1;
            return;
        }
        int index = 0;
        short bitValue = 1;
        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            HexCell neighbor = GetNeighbor(d);
            if (neighbor && neighbor.IsLand)
            {
                index += bitValue;
            }
            bitValue *= 2;
        }
        Bitmask = index;
    }
    #endregion

    #region Neighbors
    [SerializeField]
    HexCell[] neighbors = null;

    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    public HexCell GetNeighbor(HexDirection direction, bool traversableNeeded, bool freeNeeded, bool landNeeded, bool oceanNeeded, bool unitNeeded)
    {
        HexCell neighbor = GetNeighbor(direction);
        if (neighbor == null)
        {
            return null;
        }
        if (traversableNeeded && !neighbor.Traversable)
        {
            return null;
        }
        if (freeNeeded && neighbor.Unit != null)
        {
            return null;
        }
        if (landNeeded && !neighbor.IsLand)
        {
            return null;
        }
        if (oceanNeeded && !neighbor.IsOcean)
        {
            return null;
        }
        if (unitNeeded && !neighbor.Unit)
        {
            return null;
        }
        return neighbor;
    }

    public List<HexCell> GetNeighbors(bool traversableNeeded, bool freeNeeded, bool landNeeded, bool oceanNeeded, bool unitNeeded)
    {
        List<HexCell> allNeighbors = new List<HexCell>();
        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            HexCell neighbor = GetNeighbor(d, traversableNeeded, freeNeeded, landNeeded, oceanNeeded, unitNeeded);
            if (neighbor != null)
            {
                allNeighbors.Add(neighbor);
            }
        }
        return allNeighbors;
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        HexCell neighbor = neighbors[(int)direction]; //Old neighbor
        if (neighbor)
        {
            neighbor.neighbors[(int)direction.Opposite()] = null;
        }
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }
    #endregion

    #region Pathfinding
    public void ClearPathfinding()
    {
        SearchHeuristic = 0;
        NextWithSamePriority = null;
        SearchPhase = 0;
        MovementCost = 0;
        PathFrom = null;
    }

    public int SearchHeuristic { get; set; }
    public int SearchPriority
    {
        get
        {
            return MovementCost + SearchHeuristic;
        }
    }
    public HexCell NextWithSamePriority { get; set; }
    public int SearchPhase { get; set; } // 0 = not been reached | 1 = currently in searchfrontier | 2 = has been reached and taken out from frontier

    public int MovementCostPenalty { get; set; } //The hex individual cost
    public int MovementCost { get; set; } //The total cost to move here by a single unit now searching
    public HexCell PathFrom { get; set; }
    #endregion

    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
    }

    #region Grid, Highlights and Labels
    public void SetLabel(string text) => label.text = text;
    public void ShowUI(bool status) => label.enabled = status;

    public void ShowGameOutline(bool status)
    {
        if (Traversable)
        {
            gameGrid.enabled = status;
        }
        else
        {
            gameGrid.enabled = false;
        }
    }

    public void ChangeEditOutlineColor(bool traversable) => editorGrid.color = traversable ? Color.green : Color.red;

    public void ShowEditOutline(bool status) => editorGrid.enabled = status;

    public void ShowHighlight(bool status, HighlightType highlightType)
    {
        switch (highlightType)
        {
            case HighlightType.ActiveCell:
                activeCell.enabled = status;
                break;
            case HighlightType.Target:
                target.enabled = status;
                break;
            case HighlightType.PathfindingEnd:
                pathfindingEnd.enabled = status;
                break;
            case HighlightType.AbilityAffected:
                abilityAffected.enabled = status;
                break;
            case HighlightType.ValidMoveInteraction:
                validMoveInteraction.enabled = status;
                break;
            case HighlightType.ValidCombatInteraction:
                validCombatInteraction.enabled = status;
                break;
        }
    }

    public void ShowPathFrom(bool status, HexCell fromCell)
    {
        if (fromCell)
        {
            fromCell.ShowPathTo(status, this);
        }
        if (status)
        {
            visualPathFrom.enabled = fromCell != null;
            if (fromCell)
            {
                visualPathFrom.transform.right = fromCell.transform.position - this.transform.position;
            }
        }
        else
        {
            visualPathFrom.enabled = false;
        }
    }

    void ShowPathTo(bool status, HexCell toCell)
    {
        if (status)
        {
            visualPathTo.enabled = toCell != null;
            if (toCell)
            {
                visualPathTo.transform.right = toCell.transform.position - this.transform.position;
            }
            visualPathTo.enabled = visualPathTo != null;
        }
        else
        {
            visualPathTo.enabled = false;
        }
    }
    #endregion

    public void OnMouseEnter() => OnHexCellHoover?.Invoke(this);

    #region Save and Load
    public void Load(HexCellData data, HexGrid grid)
    {
        Traversable = data.traversable;
        TypeOfSpawnPos = data.spawnType;
        IsLand = data.isLand;
        Bitmask = data.bitmask;

        for (int i = 0; i < data.connected.Length; i++)
        {
            if (data.connected[i])
            {
                SetNeighbor((HexDirection)i, grid.GetCell(data.connectedCoordinates[i]));
            }
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (showNeighborGizmos)
        {
            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = GetNeighbor(d);
                if (neighbor)
                {
                    Color gizmoColor = Color.green;
                    if (!neighbor.Traversable || !this.Traversable)
                    {
                        gizmoColor = Color.red;
                    }
                    Gizmos.color = gizmoColor;
                    Gizmos.DrawLine(this.Position, neighbor.Position);
                }
            }
        }
    }
}
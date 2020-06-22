using UnityEngine;
using UnityEngine.UI;

public class HexCell : MonoBehaviour
{
    public enum OutLineType { Game, Pathfinding, Target, Editor}

    public Text label;
    public SpriteRenderer[] outlineVisuals = new SpriteRenderer[4];

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

    public enum SpawnType { Forbidden, Player, AnyEnemy, MeleeEnemy, SupportEnemy, RangedEnemy }
    public SpawnType TypeOfSpawnPos { get; set; }

    public bool showNeighborGizmos = true;

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

    #region Grid and Labels
    public void SetLabel(string text) => label.text = text;
    public void ShowUI(bool status) => label.enabled = status;

    public void ShowOutline(bool status, OutLineType outlineType)
    {
        switch (outlineType)
        {
            case OutLineType.Game:
                ShowGameOutline(status);
                break;
            case OutLineType.Pathfinding:
                ShowPathfindingOutline(status, Color.white);
                break;
            case OutLineType.Target:
                ShowTargetingOutline(status, Color.white);
                break;
            case OutLineType.Editor:
                ShowEditOutline(status);
                break;
        }
    }

    public void ShowGameOutline(bool status)
    {
        if (Traversable)
        {
            outlineVisuals[(int)OutLineType.Game].enabled = status;
        }
        else
        {
            outlineVisuals[(int)OutLineType.Game].enabled = false;
        }
    }

    public void ShowPathfindingOutline(bool status, Color color)
    {
        outlineVisuals[(int)OutLineType.Pathfinding].enabled = status;
        outlineVisuals[(int)OutLineType.Pathfinding].color = color;
    }

    public void ShowTargetingOutline(bool status, Color color)
    {
        outlineVisuals[(int)OutLineType.Target].enabled = status;
        outlineVisuals[(int)OutLineType.Target].color = color;
    }

    public void ChangeEditOutlineColor(bool traversable) => outlineVisuals[(int)OutLineType.Editor].color = traversable? Color.green : Color.red;

    public void ShowEditOutline(bool status) => outlineVisuals[(int)OutLineType.Editor].enabled = status;
    #endregion

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
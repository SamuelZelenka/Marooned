using UnityEngine;
using UnityEngine.UI;

public class HexCell : MonoBehaviour
{
    public Text label;
    public SpriteRenderer gameGrid;
    public SpriteRenderer editorGrid;
    public SpriteRenderer highlight;

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
            ChangeEditGrid(value);
        }
    }

    public enum SpawnType { Forbidden, Player, AnyEnemy, MeleeEnemy, SupportEnemy, RangedEnemy}
    SpawnType typeOfSpawnPos;
    public SpawnType TypeOfSpawnPos
    {
        get => typeOfSpawnPos;
        set
        {
            typeOfSpawnPos = value;
        }
    }

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

    public int MovementCostPenalty { get; set; }
    public int MovementCost { get; set; }
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
    public void SetLabel(string text)
    {
        label.text = text;
    }

    public void ShowUI(bool status)
    {
        label.enabled = status;
    }

    public void ShowGameGrid(bool status)
    {
        gameGrid.enabled = status;
    }

    public void ChangeEditGrid(bool traversable)
    {
        editorGrid.color = traversable ? Color.green : Color.red;
    }

    public void ShowEditGrid(bool status)
    {
        editorGrid.enabled = status;
    }

    public void SetHighlightStatus(bool status, Color color)
    {
        highlight.enabled = status;
        highlight.color = color;
    }
    #endregion

    #region Save and Load
    public void Load(HexCellData data, HexGrid grid)
    {
        Traversable = data.traversable;
        typeOfSpawnPos = data.spawnType;
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
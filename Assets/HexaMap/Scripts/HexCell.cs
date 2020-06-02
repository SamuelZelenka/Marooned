using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;
    public RectTransform uiRect;
    public Text label;
    public SpriteRenderer highlight;
    public HexUnit Unit { get; set; }

    #region Terrain
    int terrainTypeIndex;
    public int TerrainTypeIndex
    {
        get => terrainTypeIndex;
        set
        {
            if (terrainTypeIndex != value)
            {
                terrainTypeIndex = value;
            }
        }
    }

    public bool IsOcean { get; set; }
    public int GetIslandBitmask()
    {
        if (IsOcean)
        {
            Debug.LogError("Should not check island bitmask for ocean hexes");
            return -1;
        }
        int index = 0;
        short bitValue = 1;
        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            index += GetNeighbor(d).IsOcean ? bitValue * 0 : bitValue * 1;
            bitValue *= 2;
        }
        return index;
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
            return movementCost + SearchHeuristic;
        }
    }
    public HexCell NextWithSamePriority { get; set; }
    public int SearchPhase { get; set; } // 0 = not been reached | 1 = currently in searchfrontier | 2 = has been reached and taken out from frontier

    int movementCost;
    public int MovementCost
    {
        get => movementCost;
        set
        {
            movementCost = value;
        }
    }
    public HexCell PathFrom { get; set; }
    #endregion

    public Vector3 Position
    {
        get
        {
            return transform.localPosition;
        }
    }

    public void SetLabel(string text)
    {
        label.text = text;
    }

    public void ShowUI(bool status)
    {
        label.enabled = status;
    }

    public void SetHighlightStatus(bool status, Color color)
    {
        highlight.enabled = status;
        highlight.color = color;
    }

    #region Save and Load
    public void Save(BinaryWriter writer)
    {
        writer.Write((byte)terrainTypeIndex);
    }

    public void Load(BinaryReader reader)
    {
        terrainTypeIndex = reader.ReadByte();
    }
    #endregion
}
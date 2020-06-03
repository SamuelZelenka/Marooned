using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class HexGrid : MonoBehaviour
{
    public int cellCountX = 20, cellCountY = 15;

    public HexCell cellPrefab;
    public Texture2D noiseSource;
    public HexUnit unitPrefab;

    HexCell[] cells;

    public bool setTerrain = true;

    List<HexUnit> units = new List<HexUnit>();

    public int seed;
    public Tilemap terrain;
    public TileBase[] edgeTiles;
    public TileBase[] oceanTiles;
    public TileBase[] landTiles;

    private void OnEnable()
    {
        if (!HexMetrics.noiseSource)
        {
            HexMetrics.noiseSource = noiseSource;
            HexMetrics.InitializeHashGrid(seed);
            HexUnit.unitPrefab = unitPrefab;
        }
    }

    void Awake()
    {
        HexMetrics.noiseSource = noiseSource;
        HexMetrics.InitializeHashGrid(seed);

        HexUnit.unitPrefab = unitPrefab;

        CreateMap(cellCountX, cellCountY, true);
    }

    public bool CreateMap(int x, int y, bool newMap)
    {
        ClearCells();
        ClearUnits();

        cellCountX = x;
        cellCountY = y;
        CreateCells(newMap);

        ////Debug unit
        //HexUnit unit = Instantiate(unitPrefab);
        //unit.transform.position = cells[0].Position;
        //cells[0].Unit = unit;
        //unit.Location = cells[0];
        return true;
    }

   

    void CreateCells(bool newMap)
    {
        cells = new HexCell[cellCountY * cellCountX];

        for (int y = 0, i = 0; y < cellCountY; y++)
        {
            for (int x = 0; x < cellCountX; x++)
            {
                CreateCell(x, y, i++, newMap);
            }
        }

        if (newMap && setTerrain)
        {
            foreach (var item in cells)
            {
                item.CalculateBitmask();
            }
        }
    }

    void CreateCell(int x, int y, int i, bool newMap)
    {
        Vector3 position;
        position.x = (x + y * 0.5f - y / 2) * (HexMetrics.innerRadius * 2f);
        position.z = 0f;
        position.y = y * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, y);

        if (newMap)
        {
            //Connect hex neighbors to the west
            if (x > 0)
            {
                cell.SetNeighbor(HexDirection.W, cells[i - 1]);
            }
            if (y > 0)
            {
                if ((y & 1) == 0) //If even row (with X = 0 to the leftmost position)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX]);
                    if (x > 0)
                    {
                        cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX - 1]);
                    }
                }
                else //Un-even row (with X = 0 with incline into the row)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX]);
                    if (x < cellCountX - 1)
                    {
                        cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX + 1]);
                    }
                }
            }
            cell.Traversable = true;
        }

        cell.transform.SetParent(this.transform);
        cell.myGrid = this;

        if (setTerrain && newMap)
        {
            cell.IsLand = (HexMetrics.landChance > HexMetrics.SampleHashGrid(cell.Position).a);
        }
    }

    /// <summary>
    /// Paints terrain on the tilemap
    /// </summary>
    /// <param name="cell"></param>
    public void SetTerrainCellVisual(HexCell cell)
    {
        if (!setTerrain)
        {
            return;
        }
        Vector3Int tilemapPosition = HexCoordinates.CoordinatesToTilemapCoordinates(cell.coordinates);

        TileBase tile;
        int cellBitmask = cell.Bitmask;

        if (cellBitmask >= 0 && cellBitmask <= 62 )
        {
            tile  = edgeTiles[cell.Bitmask];
        }
        else if (cellBitmask < 0) //Ocean tile
        {
            tile = Utility.ReturnRandom(oceanTiles);
        }
        else //If over 62 (full land tile with all neighbors also landtiles)
        {
            tile = Utility.ReturnRandom(landTiles);
        }

        terrain.SetTile(tilemapPosition, tile);
    }

    public HexCell GetCell(HexCoordinates coordinates)
    {
        int y = coordinates.Y;
        int x = coordinates.X + y / 2;

        if (y < 0 || y >= cellCountY)
        {
            return null;
        }
        if (x < 0 || x >= cellCountX)
        {
            return null;
        }

        return cells[x + y * cellCountX];
    }

    public HexCell GetCell(Ray ray)
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider)
        {
            return GetCell(hit.point);
        }
        else
        {
            return null;
        }
    }

    public HexCell GetCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Y * cellCountX + coordinates.Y / 2;
        if (index >= 0 && cells.Length > index)
        {
            return cells[index];
        }
        else
        {
            return null;
        }
    }

    public void ShowUI(bool visible)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].ShowUI(visible);
        }
    }

    public void AddUnit(HexUnit unit, HexCell location, int orientation)
    {
        units.Add(unit);
        unit.transform.SetParent(transform, false);
        unit.Location = location;
        unit.Orientation = (HexDirection)orientation;
    }

    public void RemoveUnit(HexUnit unit)
    {
        units.Remove(unit);
        unit.Die();
    }

    public void ShowGameGrid(bool status)
    {
        foreach (var item in cells)
        {
            item.ShowGameGrid(status);
        }
    }

    public void ShowEditGrid(bool status)
    {
        foreach (var item in cells)
        {
            item.ShowEditGrid(status);
        }
    }

    public void ShowNeighborGizmos(bool status)
    {
        foreach (var item in cells)
        {
            item.showNeighborGizmos = status;
        }
    }

    void ClearUnits()
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].Die();
        }
        units.Clear();
    }

    void ClearCells()
    {
        if (cells != null)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                GameObject.Destroy(cells[i].gameObject);
            }
        }
    }

    #region Save and Load
    public HexCell[] Save()
    {
        foreach (var item in cells)
        {
            item.CalculateBitmask();
        }
        return cells;
    }

    public void Load(BattleMap map)
    {
        CreateMap(map.cellCountX, map.cellCountY, false);
        for (int i = 0; i < map.cells.Length; i++)
        {
            cells[i].Load(map.cells[i], this);
        }
    }
    #endregion
}

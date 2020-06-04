using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class HexGrid : MonoBehaviour
{
    public int cellCountX = 20, cellCountY = 15;

    [Header("References")]
    public HexCell cellPrefab;
    public Texture2D noiseSource;
    public Ship shipPrefab;

    [Header("Terrain")]
    public bool worldMap = true;
    public int seed;
    public Tilemap terrainTilemap;
    public Tilemap featuresTilemap;
    public TileBase[] edgeTiles;
    public TileBase[] oceanTiles;
    public TileBase[] landTiles;
    public TileBase[] harborTiles;

    HexCell[] cells;
    public List<HexUnit> Units { get; private set; }
    public List<HexCell> Harbors { get; private set; }

    private void OnEnable()
    {
        if (!HexMetrics.noiseSource)
        {
            HexMetrics.noiseSource = noiseSource;
            HexMetrics.InitializeHashGrid(seed);
        }
    }

    void Awake()
    {
        Units = new List<HexUnit>();
        Harbors = new List<HexCell>();

        HexMetrics.noiseSource = noiseSource;
        HexMetrics.InitializeHashGrid(seed);

        CreateMap(cellCountX, cellCountY, true);
    }

    public bool CreateMap(int x, int y, bool newMap)
    {
        ClearCells();
        ClearUnits();

        cellCountX = x;
        cellCountY = y;
        CreateCells(newMap);


        //Create DEBUG PLAYER
        CreatePlayerAndShip(cells[0], HexDirection.NE, true);

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

        if (newMap && worldMap)
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

        if (worldMap && newMap)
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
        if (!worldMap)
        {
            return;
        }
        Vector3Int tilemapPosition = HexCoordinates.CoordinatesToTilemapCoordinates(cell.coordinates);

        TileBase tile;
        int cellBitmask = cell.Bitmask;

        if (cellBitmask >= 0 && cellBitmask <= 62 ) //Edge of islands
        {
            tile  = edgeTiles[cell.Bitmask];

            //Spawn harbors
            if (HexMetrics.SampleHashGrid(cell.Position).a < HexMetrics.harborChance)
            {
                AddHarbor(cell, tilemapPosition);

                //Spawn AI ships
                if (HexMetrics.SampleHashGrid(cell.Position).b < HexMetrics.shipSpawnChance) 
                {
                    CreatePlayerAndShip(cell, (HexDirection) Random.Range((int)HexDirection.NE, (int) HexDirection.NW), false);
                }
            }
        }
        else if (cellBitmask < 0) //Ocean tile
        {
            tile = Utility.ReturnRandom(oceanTiles);
        }
        else //If over 62 (full land tile with all neighbors also landtiles)
        {
            tile = Utility.ReturnRandom(landTiles);
        }

        terrainTilemap.SetTile(tilemapPosition, tile);
    }

    public void AddHarbor(HexCell cell, Vector3Int tilemapPosition)
    {
        cell.HasHarbor = true;
        Harbors.Add(cell);

        List<HexDirection> openwaterConnections = new List<HexDirection>();
        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            HexCell neighbor = cell.GetNeighbor(d);
            if (neighbor && neighbor.IsOcean)
            {
                openwaterConnections.Add(d);
            }
        }
        HexDirection dir = Utility.ReturnRandom(openwaterConnections);
        featuresTilemap.SetTile(tilemapPosition, harborTiles[(int)dir]);
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

    private void CreatePlayerAndShip(HexCell location, HexDirection direction, bool playerControlled)
    {
        Ship ship = Instantiate(shipPrefab);
        AddUnit(ship, location, direction, playerControlled);
        List<HexUnit> playerUnits = new List<HexUnit>();
        playerUnits.Add(ship);
        Player newPlayer = new Player(playerUnits, playerControlled);
        HexGridController.instance.AddPlayerToTurnOrder(newPlayer);
    }

    private void AddUnit(HexUnit unit, HexCell location, HexDirection orientation, bool playerControlled)
    {
        Units.Add(unit);
        location.Unit = unit;
        unit.transform.SetParent(transform, false);
        unit.Location = location;
        unit.Orientation = (HexDirection)orientation;
        unit.playerControlled = playerControlled;
        unit.myGrid = this;
    }

    public void RemoveUnit(HexUnit unit)
    {
        Units.Remove(unit);
        unit.Location.Unit = null;
        unit.Die();
    }

    public void ShowGameGrid(bool status)
    {
        foreach (var item in cells)
        {
            item.ShowGameGrid(status);
        }
    }

    #region UI and Grid
    public void ShowUI(bool visible)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].ShowUI(visible);
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
    #endregion

    void ClearUnits()
    {
        for (int i = 0; i < Units.Count; i++)
        {
            Units[i].Die();
        }
        Units.Clear();
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

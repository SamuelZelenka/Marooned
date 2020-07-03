﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class HexGrid : MonoBehaviour
{
    public int CellCountX { private set; get; }
    public int CellCountY { private set; get; }

    [Header("References")]
    public HexCell cellPrefab;
    public Texture2D noiseSource;
    public InGameCamera cameraController;

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

    static bool gameGridStatus = true;

    private void OnEnable()
    {
        if (!HexMetrics.noiseSource)
        {
            HexMetrics.noiseSource = noiseSource;
            HexMetrics.InitializeHashGrid(seed);
        }

        ShowGameGrid(gameGridStatus);
    }

    void Awake()
    {
        Units = new List<HexUnit>();
        Harbors = new List<HexCell>();

        if (!HexMetrics.noiseSource)
        {
            HexMetrics.noiseSource = noiseSource;
            HexMetrics.InitializeHashGrid(seed);
        }
    }

    public bool CreateMap(int x, int y, bool newMap, bool defaultTraversable, bool destroyUnits)
    {
        ClearCells();
        if (destroyUnits)
        {
            ClearUnits();
        }

        CellCountX = x;
        CellCountY = y;
        CreateCells(newMap, defaultTraversable);

        if (!destroyUnits)
        {
            ReAddUnits(Units);
        }

        ShowGameGrid(gameGridStatus);

        return true;
    }

    void CreateCells(bool newMap, bool defaultTraversable)
    {
        cells = new HexCell[CellCountY * CellCountX];

        for (int y = 0, i = 0; y < CellCountY; y++)
        {
            for (int x = 0; x < CellCountX; x++)
            {
                CreateCell(x, y, i++, newMap, defaultTraversable);
            }
        }

        if (newMap && worldMap)
        {
            foreach (var item in cells)
            {
                item.CalculateBitmask();

                if (item.IsLand)
                {
                    //Spawn harbors
                    if (HexMetrics.SampleHashGrid(item.Position).a < HexMetrics.harborChance)
                    {
                        AddHarbor(item);
                    }
                }
            }
        }

        SetCameraBoundriesToMatchHexGrid();
    }

    void CreateCell(int x, int y, int i, bool newMap, bool defaultTraversable)
    {
        Vector3 position;
        position.x = (x + y * 0.5f - y / 2) * (HexMetrics.innerRadius * 2f);
        position.z = 0f;
        position.y = y * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(this.transform);
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
                    cell.SetNeighbor(HexDirection.SE, cells[i - CellCountX]);
                    if (x > 0)
                    {
                        cell.SetNeighbor(HexDirection.SW, cells[i - CellCountX - 1]);
                    }
                }
                else //Un-even row (with X = 0 with incline into the row)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - CellCountX]);
                    if (x < CellCountX - 1)
                    {
                        cell.SetNeighbor(HexDirection.SE, cells[i - CellCountX + 1]);
                    }
                }
            }
            cell.Traversable = defaultTraversable;
        }

        cell.myGrid = this;

        if (worldMap && newMap)
        {
            cell.IsLand = (HexMetrics.landChance > HexMetrics.SampleHashGrid(cell.Position).a);
        }
    }

    public void SetCameraBoundriesToMatchHexGrid()
    {
        Vector3 minPos = cells[0].transform.position;
        minPos.x -= HexMetrics.innerRadius;
        minPos.y -= HexMetrics.outerRadius;

        Vector3 maxPos = cells[cells.Length - 1].transform.position;

        maxPos.x = Mathf.Max(maxPos.x, cells[CellCountX * 2 - 1].transform.position.x); //Allow camera movement to the rightmost position (even rows goes further to the right than un-even rows)

        maxPos.x += HexMetrics.innerRadius;
        maxPos.y += HexMetrics.outerRadius;

        cameraController.SetBoundries(minPos, maxPos);
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

        if (cellBitmask >= 0 && cellBitmask <= 62) //Edge of islands
        {
            tile = edgeTiles[cell.Bitmask];
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

    public void AddHarbor(HexCell cell)
    {
        Vector3Int tilemapPosition = HexCoordinates.CoordinatesToTilemapCoordinates(cell.coordinates);

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

        if (y < 0 || y >= CellCountY)
        {
            return null;
        }
        if (x < 0 || x >= CellCountX)
        {
            return null;
        }

        return cells[x + y * CellCountX];
    }

    public HexCell GetCell()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider)
        {
            HexCell foundCell = hit.collider.GetComponent<HexCell>();
            return foundCell;
        }
        else
        {
            return null;
        }
    }

    public List<HexCell> GetAllCells(bool traversable, bool hasUnit)
    {
        List<HexCell> foundCells = new List<HexCell>();
        foreach (var item in cells)
        {
            if (traversable && !item.Traversable)
            {
                continue;
            }
            if (hasUnit && item.Unit == null)
            {
                continue;
            }
            foundCells.Add(item);
        }
        return foundCells;
    }

    public HexCell GetRandomFreeCell()
    {
        HexCell cell = null;

        List<HexCell> cellsToTest = new List<HexCell>();
        cellsToTest.AddRange(cells);

        bool allowedCell = false;
        while (!allowedCell && cellsToTest.Count > 0)
        {
            cell = Utility.ReturnRandom(cellsToTest);
            cellsToTest.Remove(cell);

            if (cell != null && cell.Unit == null && cell.Traversable)
            {
                allowedCell = true;
            }
        }

        if (!allowedCell)
        {
            Debug.LogWarning("");
        }
        return cell;
    }

    public HexCell GetRandomFreeHarbor()
    {
        HexCell cell = null;

        List<HexCell> cellsToTest = new List<HexCell>();
        cellsToTest.AddRange(Harbors);

        bool allowedCell = false;
        while (!allowedCell && cellsToTest.Count > 0)
        {
            cell = Utility.ReturnRandom(Harbors);
            cellsToTest.Remove(cell);

            if (cell != null && cell.Unit == null && cell.Traversable)
            {
                allowedCell = true;
            }
        }

        if (!allowedCell)
        {
            Debug.LogWarning("Could not find a free cell of the requested spawntype");
        }
        return cell;
    }

    public HexCell GetFreeCellForCharacterSpawn(HexCell.SpawnType spawnTypeRequest)
    {
        HexCell cell = null;

        List<HexCell> cellsToTest = new List<HexCell>();
        cellsToTest.AddRange(cells);

        bool allowedCell = false;
        while (!allowedCell && cellsToTest.Count > 0)
        {
            cell = Utility.ReturnRandom(cellsToTest);
            cellsToTest.Remove(cell);

            if (cell != null && cell.Unit == null && cell.TypeOfSpawnPos == spawnTypeRequest && cell.Traversable)
            {
                allowedCell = true;
            }
        }

        if (!allowedCell)
        {
            Debug.LogWarning("Could not find a free cell of the requested spawntype");
        }
        return cell;
    }

    #region Units
    public void AddUnit(HexUnit unit, HexCell location, HexDirection orientation, bool playerControlled)
    {
        Units.Add(unit);
        location.Unit = unit;
        unit.Location = location;
        unit.Orientation = (HexDirection)orientation;
        unit.playerControlled = playerControlled;
        unit.myGrid = this;
    }

    public void AddUnit(HexUnit unit, HexCell location, bool playerControlled)
    {
        Units.Add(unit);
        location.Unit = unit;
        unit.Location = location;
        unit.playerControlled = playerControlled;
        unit.myGrid = this;
    }

    private void ReAddUnits(List<HexUnit> units)
    {
        List<HexUnit> unitsToReAdd = new List<HexUnit>();
        unitsToReAdd.AddRange(units);

        units.Clear();

        foreach (var item in unitsToReAdd)
        {
            AddUnit(item, GetCell(item.Location.coordinates), item.playerControlled);
        }
    }

    public void RemoveUnit(HexUnit unit)
    {
        Units.Remove(unit);
        unit.Location.Unit = null;
        unit.Die();
    }
    #endregion

    #region UI and Grid
    public void ShowUI(bool visible)
    {
        foreach (var item in cells)
        {
            item.ShowUI(visible);
        }
    }

    public void ShowGameGrid(bool status)
    {
        if (cells == null)
        {
            return;
        }
        gameGridStatus = status;
        foreach (var item in cells)
        {
            item.ShowGameOutline(status);
        }
    }

    public void ShowEditGrid(bool status)
    {
        foreach (var item in cells)
        {
            item.ShowEditOutline(status);
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
            Units[i].Despawn();
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

    public void ClearSearchHeuristics()
    {
        foreach (var item in cells)
        {
            item.ClearPathfinding();
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

    public void Load(BattleMap map, bool destroyUnits)
    {
        CreateMap(map.cellCountX, map.cellCountY, false, false, destroyUnits);
        for (int i = 0; i < map.cells.Length; i++)
        {
            cells[i].Load(map.cells[i], this);
        }
        ShowGameGrid(gameGridStatus);
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainMap : MonoBehaviour
{
    HexGrid hexGrid;
    MerchantRoute[] merchantRoutes;
    public List<HexCell> Harbors { private set; get; }

    [Header("Visuals")]
    public TileBase[] edgeTiles;
    public TileBase[] oceanTiles;
    public TileBase[] landTiles;
    public TileBase[] harborTiles;
    public Tilemap terrainTilemap;
    public Tilemap featuresTilemap;

    [Header("Merchants and AI")]
    public Transform shipParent;
    public MerchantShip aiMerchantShip;

    public void Setup(HexGrid hexGrid, int numberOfMerchantRoutes)
    {
        this.hexGrid = hexGrid;
        merchantRoutes = new MerchantRoute[numberOfMerchantRoutes];
        Harbors = new List<HexCell>();

        for (int i = 0; i < merchantRoutes.Length; i++)
        {
            //Create route between harbors : TODO

            //Spawn Merchant ships on merchant routes : TODO

        }
        CreateMap();
    }

    void CreateMap()
    {
        foreach (HexCell cell in hexGrid.Cells)
        {
            cell.IsLand = (HexMetrics.landChance > HexMetrics.SampleHashGrid(cell.Position).a);
        }

        foreach (HexCell cell in hexGrid.Cells)
        {
            cell.CalculateBitmask();
            SetTerrainCellVisual(cell);

            if (cell.IsShore)
            {
                //Spawn harbors
                if (HexMetrics.SampleHashGrid(cell.Position).a < HexMetrics.harborChance)
                {
                    AddHarbor(cell);
                }
            }
        }
    }

    void AddHarbor(HexCell cell)
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

    /// <summary>
    /// Paints terrain on the tilemap
    /// </summary>
    /// <param name="cell"></param>
    public void SetTerrainCellVisual(HexCell cell)
    {
        Vector3Int tilemapPosition = HexCoordinates.CoordinatesToTilemapCoordinates(cell.coordinates);

        TileBase tile;
        int cellBitmask = cell.Bitmask;

        if (cell.IsShore) //Edge of islands
        {
            tile = edgeTiles[cell.Bitmask];
        }
        else if (cell.IsOcean) //Ocean tile
        {
            tile = Utility.ReturnRandom(oceanTiles);
        }
        else //If over 62 (full land tile with all neighbors also landtiles)
        {
            tile = Utility.ReturnRandom(landTiles);
        }

        terrainTilemap.SetTile(tilemapPosition, tile);
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

    //Creates an AI controlled merchant player from a prefab and spawns a ship and adds it to the controller
    private void CreateMerchantPlayer()
    {
        Ship newShip = Instantiate(aiMerchantShip);
        newShip.transform.SetParent(shipParent);

        hexGrid.AddUnit(newShip, GetRandomFreeHarbor(), HexDirectionExtension.ReturnRandomDirection(), false);

        Player newMerchantPlayer = new Player(newShip, false);
        MapTurnSystem.instance.AddPlayerToTurnOrder(newMerchantPlayer);
    }
}

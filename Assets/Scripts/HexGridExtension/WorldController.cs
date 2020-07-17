using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldController : MonoBehaviour
{
    HexGrid hexGrid;
    MerchantRoute[] merchantRoutes;
    public List<HexCell> Harbors { private set; get; }
    public List<Landmass> Landmasses { private set; get; }

    [Header("Visuals")]
    public TileBase[] edgeTiles;
    public TileBase[] oceanTiles;
    public TileBase[] landTiles;
    public TileBase[] harborTiles;
    public Tilemap terrainTilemap;
    public Tilemap featuresTilemap;

    [Header("Merchants and AI")]
    [SerializeField] Transform shipParent = null;
    [SerializeField] MerchantShip aiMerchantShip = null;

    [Header("Misc References")]
    [SerializeField] WorldUIView worldUIView = null;

    public void Setup(HexGrid hexGrid, SetupData setupData)
    {
        this.hexGrid = hexGrid;
        merchantRoutes = new MerchantRoute[setupData.numberOfMerchantRoutes];
        Harbors = new List<HexCell>();
        Landmasses = new List<Landmass>();

        CreateMap(setupData);

        //Add temporary ship to do pathfinding and finding harbors
        MerchantShip setupShip = Instantiate(aiMerchantShip.gameObject).GetComponent<MerchantShip>();
        hexGrid.AddUnit(setupShip, Harbors[0], false);

        for (int i = 0; i < merchantRoutes.Length; i++)
        {
            int tries = 0;

            //Find harbors for route
            HexCell startHarbor = GetRandomFreeHarbor();
            setupShip.Location = startHarbor;

            int numberOfHarbors = Random.Range(setupData.merchantRouteMinHarbors, setupData.merchantRouteMaxHarbors + 1);
            List<HexCell> harbors = new List<HexCell>(numberOfHarbors);
            harbors.Add(startHarbor);
            for (int j = 1; j < numberOfHarbors; j++)
            {
                HexCell foundHarbor = GetRandomHarborWithinRange(setupShip, setupData.merchantRouteMinLength, setupData.merchantRouteMaxLength);
                if (foundHarbor == null)
                {
                    break;
                }
                harbors.Add(foundHarbor);
            }

            if (harbors.Count < 2)
            {
                Debug.Log("Did not find enough harbors to add to route, trying with a differnt start harbor");
                i--;
                tries++;
                if (tries < setupData.numberOfMerchantRoutes * 2)
                {
                    break;
                }
                else
                {
                    continue;
                }
            }

            //Create route between harbors
            merchantRoutes[i] = new MerchantRoute(harbors.ToArray());

            //Spawn a Merchant ship on the new route
            CreateMerchantPlayer(merchantRoutes[i]);
        }

        //Remove Temporary Ship
        hexGrid.RemoveUnit(setupShip);
    }

    void CreateMap(SetupData setupData)
    {
        //LAND OR OCEAN
        foreach (HexCell cell in hexGrid.Cells)
        {
            List<HexCell> neighborLandTiles = new List<HexCell>();
            neighborLandTiles.PopulateListWithMatchingConditions(cell.Neighbors, (c) => c.IsLand == true);
            if (neighborLandTiles.Count == 0)
            {
                bool isNewLand = setupData.newLandmassChance > HexMetrics.SampleHashGrid(cell.Position).a;
                cell.IsLand = isNewLand;
                if (isNewLand)
                {
                    Landmass newLandmass = new Landmass();
                    Landmasses.Add(newLandmass);
                    cell.Landmass = newLandmass;
                }
            }
            else
            {
                bool isLand = setupData.landByLandChance * neighborLandTiles.Count > HexMetrics.SampleHashGrid(cell.Position).a;

                //Add all nearby landmasses to list
                List<Landmass> neighboringLandmasses = new List<Landmass>();
                foreach (var landCell in neighborLandTiles)
                {
                    if (neighboringLandmasses.Contains(landCell.Landmass))
                    {
                        continue;
                    }
                    neighboringLandmasses.Add(landCell.Landmass);
                }
                //Get size of all nearby landmasses
                int totalSize = 0;
                foreach (var landmass in neighboringLandmasses)
                {
                    totalSize += landmass.landCells.Count;
                }
                //If exceeding maximum landmasssize after conversion to land (and combining any landmasses previously not connected, then abort
                if (totalSize + 1 > setupData.landMassMaxSize)
                {
                    isLand = false;
                }

                cell.IsLand = isLand;
                if (isLand)
                {
                    cell.Landmass = neighborLandTiles[0].Landmass;
                }
            }
        }

        //SET HEXES SURROUNDED BY LAND AS LAND
        foreach (HexCell cell in hexGrid.Cells)
        {
            List<HexCell> neighborLandTiles = new List<HexCell>();
            neighborLandTiles.PopulateListWithMatchingConditions(cell.Neighbors, (c) => c.IsLand == true);
            if (neighborLandTiles.Count == 6)
            {
                cell.IsLand = true;
                cell.Landmass = neighborLandTiles[0].Landmass;
            }
        }

        //Clear empty landmasses
        for (int i = 0; i < Landmasses.Count; i++)
        {
            if (Landmasses[i].landCells.Count < 1)
            {
                Landmasses.RemoveAt(i);
                i--;
            }
        }

        //BITMASK AND VISUALS
        foreach (HexCell cell in hexGrid.Cells)
        {
            cell.CalculateBitmask();
            SetTerrainCellVisual(cell);
        }

        //HARBORS AND POINTS OF INTERESTS
        foreach (Landmass landmass in Landmasses)
        {
            HexCell harborCell = Utility.ReturnRandom(landmass.GetShores());
            landmass.harbor = harborCell;
            AddHarbor(harborCell, setupData);
        }
    }

    void AddHarbor(HexCell cell, SetupData setupData)
    {
        Vector3Int tilemapPosition = HexCoordinates.CoordinatesToTilemapCoordinates(cell.coordinates);

        cell.HasHarbor = true;
        Harbors.Add(cell);
        string harborName = setupData.islandNames[0];
        if (setupData.islandNames.Count > 1)
        {
            setupData.islandNames.RemoveAt(0);
        }
        cell.PointOfInterest = new Harbor(harborName, worldUIView.EnablePOIButton);

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

        while (cellsToTest.Count > 0)
        {
            cell = Utility.ReturnRandom(cellsToTest);

            if (cell != null && cell.Unit == null && cell.Traversable)
            {
                return cell;
            }
            cellsToTest.Remove(cell);
        }
        Debug.LogWarning("Could not find a free cell of the requested spawntype");
        return null;
    }

    public HexCell GetRandomHarborWithinRange(Ship ship, int minDistance, int maxDistance)
    {
        HexCell cell = null;

        List<HexCell> cellsToTest = new List<HexCell>();
        cellsToTest.AddRange(Harbors);

        while (cellsToTest.Count > 0)
        {
            cell = Utility.ReturnRandom(cellsToTest);

            Pathfinding.FindPath(ship.Location, cell, ship, false);
            List<HexCell> pathToHarbor = Pathfinding.GetWholePath();

            if (cell != null && cell.Traversable && pathToHarbor != null && pathToHarbor.Count >= minDistance && pathToHarbor.Count <= maxDistance)
            {
                return cell;
            }
            cellsToTest.Remove(cell);
        }
        Debug.LogWarning("Could not find a free cell of the requested spawntype");
        return null;
    }

    //Creates an AI controlled merchant player from a prefab and spawns a ship and adds it to the controller
    private void CreateMerchantPlayer(MerchantRoute route)
    {
        MerchantShip newShip = Instantiate(aiMerchantShip);
        newShip.transform.SetParent(shipParent);
        newShip.Setup(route);

        hexGrid.AddUnit(newShip, route.GetSpawnableLocation(), HexDirectionExtension.ReturnRandomDirection(), false);

        Player newMerchantPlayer = new Player(newShip, false);
        MapTurnSystem.instance.AddPlayerToTurnOrder(newMerchantPlayer);
    }
}

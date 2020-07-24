using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldSetup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] MerchantShip merchantShip = null;
    [SerializeField] WorldUIView worldUIView = null;
    [SerializeField] Transform oceanSquareParent = null;

    [Header("Visuals")]
    [SerializeField] TileBase[] edgeTiles = null;
    //[SerializeField] TileBase[] oceanTiles; //We do not use ocean tiles/hexes in this game for now
    [SerializeField] TileBase[] landTiles = null;
    [SerializeField] TileBase[] harborTiles = null;
    [SerializeField] Tilemap terrainTilemap = null;
    [SerializeField] Tilemap featuresTilemap = null;
    [SerializeField] GameObject oceanSquarePrefab = null;

    public void Setup(HexGrid hexGrid, SetupData setupData, WorldController worldController)
    {
        CreateWater(setupData);
        CreateMap(hexGrid, setupData, worldController);

        //Add temporary ship to do pathfinding and finding harbors
        MerchantShip setupShip = Instantiate(merchantShip.gameObject).GetComponent<MerchantShip>();
        hexGrid.AddUnit(setupShip, worldController.HarborCells[0], false);

        for (int i = 0; i < worldController.MerchantRoutes.Length; i++)
        {
            int tries = 0;

            //Find harbors for route
            HexCell startHarbor = Utility.ReturnRandomElementWithCondition(worldController.HarborCells, (h) => h.Unit != null);
            setupShip.Location = startHarbor;

            int numberOfHarbors = Random.Range(setupData.merchantRouteMinHarbors, setupData.merchantRouteMaxHarbors + 1);
            List<HexCell> harbors = new List<HexCell>(numberOfHarbors);
            harbors.Add(startHarbor);
            for (int j = 1; j < numberOfHarbors; j++)
            {
                HexCell foundHarbor = worldController.GetRandomHarborWithinRange(setupShip, setupData.merchantRouteMinLength, setupData.merchantRouteMaxLength);
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
            worldController.MerchantRoutes[i] = new Route(harbors.ToArray());
        }
        //Remove Temporary Ship
        hexGrid.RemoveUnit(setupShip);
    }

    const int EXTRAWATER = 2;
    void CreateWater(SetupData setupData)
    {
        for (int i = -EXTRAWATER; i < setupData.mapCellCountX + EXTRAWATER; i++)
        {
            for (int j = -EXTRAWATER; j < setupData.mapCellCountY + EXTRAWATER; j++)
            {
                GameObject newOceanSquare = Instantiate(oceanSquarePrefab);
                Vector2 position = new Vector2(i, j);
                newOceanSquare.transform.SetParent(oceanSquareParent);
                newOceanSquare.transform.position = position;
            }
        }
    }

    void CreateMap(HexGrid hexGrid,SetupData setupData, WorldController worldController)
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
                    worldController.Landmasses.Add(newLandmass);
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
        for (int i = 0; i < worldController.Landmasses.Count; i++)
        {
            if (worldController.Landmasses[i].landCells.Count < 1)
            {
                worldController.Landmasses.RemoveAt(i);
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
        foreach (Landmass landmass in worldController.Landmasses)
        {
            HexCell harborCell = Utility.ReturnRandom(landmass.GetShores());
            landmass.harbor = harborCell;
            AddHarbor(harborCell, setupData, worldController);
        }
    }

    void AddHarbor(HexCell cell, SetupData setupData, WorldController worldController)
    {
        Vector3Int tilemapPosition = HexCoordinates.CoordinatesToTilemapCoordinates(cell.coordinates);

        cell.HasHarbor = true;
        worldController.HarborCells.Add(cell);
        string harborName = setupData.islandNames[0];
        if (setupData.islandNames.Count > 1)
        {
            setupData.islandNames.RemoveAt(0);
        }

        bool hasTavern = setupData.harborTavernChance > HexMetrics.SampleHashGrid(cell.Position).b;
        bool hasMerchant = setupData.harborMerchantChance > HexMetrics.SampleHashGrid(cell.Position).c;

        cell.PointOfInterest = new Harbor(harborName, worldUIView.EnablePOIInteraction, hasMerchant, hasTavern);

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
            //tile = Utility.ReturnRandom(oceanTiles); //For now ignore ocean tiles
            tile = null;
        }
        else //If over 62 (full land tile with all neighbors also landtiles)
        {
            tile = Utility.ReturnRandom(landTiles);
        }

        terrainTilemap.SetTile(tilemapPosition, tile);
    }

}

using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    HexGrid hexGrid;
    DifficultySettings difficultySettings;
    public Route[] MerchantRoutes { private set; get; }
    public List<Harbor> Harbors
    {
        get
        {
            List<Harbor> harbors = new List<Harbor>();
            foreach (var harborCell in HarborCells)
            {
                if (harborCell.PointOfInterest is Harbor)
                {
                    harbors.Add(harborCell.PointOfInterest as Harbor);
                }
            }
            return harbors;
        }
    }
    public List<HexCell> HarborCells { private set; get; }

    public List<Landmass> Landmasses { private set; get; }
    public HexCell PlayerSpawnPosition { private set; get; }

    [Header("Ships and AI")]
    [SerializeField] Transform shipParent = null;
    [SerializeField] MerchantShip merchantShip = null;
    [SerializeField] PatrolShip redcoatShip = null;

    [Header("Misc References")]
    [SerializeField] WorldUIView worldUIView = null;
    [SerializeField] CombatSystem combatSystem = null;
    [SerializeField] WorldSetup worldSetup = null;
    [SerializeField] MapTurnSystem turnSystem = null;

    [SerializeField] List<Character> allCharacters = null;

    private void OnEnable()
    {
        turnSystem.OnWeekEnded += ChangeCharactersInTaverns;
    }

    private void OnDisable()
    {
        turnSystem.OnWeekEnded -= ChangeCharactersInTaverns;
    }

    public void Setup(HexGrid hexGrid, SetupData setupData)
    {
        turnSystem.OnFirstTurnStarted += ChangeCharactersInTaverns;

        this.hexGrid = hexGrid;
        difficultySettings = setupData.difficultySettings;

        MerchantRoutes = new Route[setupData.numberOfMerchantRoutes];
        HarborCells = new List<HexCell>();
        Landmasses = new List<Landmass>();
        worldSetup.Setup(hexGrid, setupData, this);

        foreach (var route in MerchantRoutes)
        {
            //Spawn a Merchant ship on the new route
            CreateMerchantPlayer(route);

            //Spawn a Redcoat patrolship on the new route
            CreateRedcoatPlayer(route);
        }
        //Set player spawn position
        PlayerSpawnPosition = Utility.ReturnRandom(CellFinder.GetCellsWithinRange(HarborCells[0], 2, (c) => c.Traversable == true, (c) => c.Unit == null, (c) => c.IsOcean == true));
    }

    public HexCell GetRandomHarborWithinRange(Ship ship, int minDistance, int maxDistance)
    {
        HexCell cell = null;

        List<HexCell> cellsToTest = new List<HexCell>();
        cellsToTest.AddRange(HarborCells);

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

    //Creates an AI controlled merchant player with a ship and adds it to controllers
    private void CreateMerchantPlayer(Route route)
    {
        MerchantShip newShip = Instantiate(merchantShip);
        newShip.transform.SetParent(shipParent);
        newShip.Setup(route);

        //Delegates
        newShip.OnShipBoarded += worldUIView.OpenBoardingView;
        newShip.OnShipInspected += worldUIView.OpenInspectView;

        hexGrid.AddUnit(newShip, route.GetSpawnableLocation(), HexDirectionExtension.ReturnRandomDirection(), false);

        Player newMerchantPlayer = new Player(newShip, false);
        MapTurnSystem.instance.AddPlayerToTurnOrder(newMerchantPlayer);
    }

    //Creates an AI controlled Redcoat player with a ship and adds it to controllers
    private void CreateRedcoatPlayer(Route route)
    {
        PatrolShip newShip = Instantiate(redcoatShip);
        newShip.transform.SetParent(shipParent);
        newShip.Setup(route);

        //Delegates
        newShip.OnShipBoarded += combatSystem.StartCombat;
        newShip.OnShipInspected += worldUIView.OpenInspectView;

        hexGrid.AddUnit(newShip, route.GetSpawnableLocation(), HexDirectionExtension.ReturnRandomDirection(), false);

        Player newRedcoatPlayer = new Player(newShip, false);
        MapTurnSystem.instance.AddPlayerToTurnOrder(newRedcoatPlayer);
    }

    public int GetVisionRange(PlayerData playerData)
    {
        return Mathf.FloorToInt((float) playerData.Bounty / difficultySettings.visionRangeToBounty);
    }

    void ChangeCharactersInTaverns()
    {
        Debug.Log("Characters in taverns changed locations");
        Player player = HexGridController.player;
        List<Character> recruitableCharacters = new List<Character>();
        List<Harbor> harborsWithTavern = new List<Harbor>();
        foreach (var harbor in Harbors)
        {
            if (harbor.hasTavern)
            {
                harborsWithTavern.Add(harbor);
            }
        }

        foreach (var character in allCharacters)
        {
            bool isRecruitable = true;
            for (int i = 0; i < player.Crew.Count; i++)
            {
                if (character.characterData.ID == player.Crew[i].characterData.ID)
                {
                    isRecruitable = false;
                }
            }
            for (int i = 0; i < player.PlayerData.DeadCharacters.Count; i++)
            {
                if (character.characterData.ID == player.PlayerData.DeadCharacters[i].ID)
                {
                    isRecruitable = false;
                }
            }
            if (isRecruitable)
            {
                recruitableCharacters.Add(character);
            }
        }

        foreach (var character in recruitableCharacters)
        {
            Harbor harbor = Utility.ReturnRandom(harborsWithTavern);
            harbor.recruitableCharacter = character;
            harborsWithTavern.Remove(harbor);
        }
    }
}
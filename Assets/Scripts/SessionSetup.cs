using UnityEngine;
using System.Collections.Generic;

public class SessionSetup : MonoBehaviour
{
    [Header("Map and references")]
    public HexGrid worldGrid;
    public HexGrid shipGrid;
    public Transform shipTransform;
    public Transform playerCrewParent;
    public WorldController worldController;
    public Texture2D noiseSource;

    [Header("Player setup")]
    public Ship playerStarterShip;
    public BattleMap playerStartingGridMap;
    public CrewSimulation playerCrewSimulation;
    public Character[] startingCharacters;
    public CombatSystem combatSystem;
    public QuestController questController;

    public delegate void SessionSetupHandler(Player player);
    public static event SessionSetupHandler OnPlayerCreated;

    public SetupData setupData;

    // Start is called before the first frame update
    void Start() => ConfirmSetup();

    //Used to confirm the settings of the game session
    public void ConfirmSetup()
    {
        setupData.SetSeed();
        if (!HexMetrics.noiseSource)
        {
            HexMetrics.noiseSource = noiseSource;
            HexMetrics.InitializeHashGrid(setupData.Seed);
        }

        worldGrid.CreateMap(setupData.mapCellCountX, setupData.mapCellCountY, true, true, true);
        worldController.Setup(worldGrid, setupData);
        shipGrid.Load(playerStartingGridMap, true);

        worldGrid.SetCameraBoundriesToMatchHexGrid();
        shipGrid.SetCameraBoundriesToMatchHexGrid();

        HexGridController.worldGrid = worldGrid;
        HexGridController.playerShipGrid = shipGrid;
        HexGridController.playerCrewParent = playerCrewParent;

        CreateHumanPlayer();
        MapTurnSystem.instance.DoFirstTurn();

        questController.GiveFirstQuest();
    }

    //Creates a player from a prefab and spawns a ship, starting characters and adds them to controllers 
    private void CreateHumanPlayer()
    {
        Ship newShip = Instantiate(playerStarterShip);
        newShip.transform.SetParent(shipTransform);

        worldGrid.AddUnit(newShip, worldController.PlayerSpawnPosition, HexDirectionExtension.ReturnRandomDirection(), true);

        combatSystem.managementMap = playerStartingGridMap;

        Player newPlayer = new Player(newShip, true, playerCrewSimulation);
        MapTurnSystem.instance.AddPlayerToFirstPositionInTurnOrder(newPlayer);
        HexGridController.player = newPlayer;
        newPlayer.PlayerData.NextBountyChange = setupData.difficultySettings.bountyChanges;
        for (int i = 0; i < startingCharacters.Length; i++)
        {
            HexGridController.SpawnCharacterForPlayerCrew(startingCharacters[i]);
        }
        OnPlayerCreated?.Invoke(newPlayer);
    }
}

[System.Serializable]
public class SetupData
{
    public DifficultySettings difficultySettings;
    const int SEEDMAXCHARS = 10;
    public int Seed { get; private set; }
    public string stringSeed;
    public int mapCellCountX = 20, mapCellCountY = 15;
    public int numberOfMerchantRoutes = 5;
    public int merchantRouteMinLength = 10;
    public int merchantRouteMaxLength = 20;
    public int merchantRouteMinHarbors = 2;
    public int merchantRouteMaxHarbors = 4;
    public float newLandmassChance = 0.1f;
    public float landByLandChance = 0.25f;
    public int landMassMaxSize = 15;
    public List<string> islandNames = new List<string>(1);
    public float harborMerchantChance = 0.9f;
    public float harborTavernChance = 0.6f;


    public void SetSeed()
    {
        char[] chars = stringSeed.ToCharArray();
        for (int i = 0; i < chars.Length && i < SEEDMAXCHARS; i++)
        {
            Seed += chars[i].GetHashCode();

        }
    }
}
[System.Serializable]
public class DifficultySettings
{
    public int bountyChanges = 500;
    public int BountyToVisionRange
    {
        get => bountyChanges;
    }
    public int BountyToCrewSize
    {
        get => bountyChanges * 2;
    }

    public int minimumEnemyCrewSize = 2;
    public int maximumEnemyCrewSize = 10;
}
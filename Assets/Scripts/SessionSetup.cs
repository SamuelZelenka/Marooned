using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionSetup : MonoBehaviour
{
    [Header("Map and references")]
    public HexGrid terrainGrid;
    public HexGrid shipGrid;
    public Transform shipTransform;
    public Transform playerCrewParent;

    public int mapCellCountX = 20, mapCellCountY = 15;

    [Header("Player setup")]
    public Ship playerStarterShip;
    public BattleMap playerStartingGridMap;
    public CrewSimulation playerCrewSimulation;
    public Character startingCharacter;
    public int numberOfStartingCharacters = 1;
    public CombatSystem combatSystem;

    [Header("AI setup")]
    public GameObject aiPrefab;
    public Ship aiMerchantShip;
    public int numberOfMerchantShips = 5;

    public delegate void PlayerSetup(Player player);
    public static PlayerSetup OnHumanPlayerCreated;

    // Start is called before the first frame update
    void Start() => ConfirmSetup();

    //Used to confirm the settings of the game session
    public void ConfirmSetup()
    {
        terrainGrid.CreateMap(mapCellCountX, mapCellCountY, true, true, true);
        shipGrid.Load(playerStartingGridMap, true);

        terrainGrid.SetCameraBoundriesToMatchHexGrid();
        shipGrid.SetCameraBoundriesToMatchHexGrid();

        CreateHumanPlayer();

        for (int i = 0; i < numberOfMerchantShips; i++)
        {
            CreateMerchantPlayer();
        }

        MapTurnSystem.instance.DoFirstTurn();
    }

    //Creates a player from a prefab and spawns a ship and adds it to the controller
    private void CreateHumanPlayer()
    {
        Ship newShip = Instantiate(playerStarterShip);
        newShip.transform.SetParent(shipTransform);

        terrainGrid.AddUnit(newShip, terrainGrid.GetRandomFreeHarbor(), HexDirectionExtension.ReturnRandomDirection(), true);

        combatSystem.managementMap = playerStartingGridMap;

        Player newPlayer = new Player(newShip, true, playerCrewSimulation);
        MapTurnSystem.instance.AddPlayerToTurnOrder(newPlayer);

        for (int i = 0; i < numberOfStartingCharacters; i++)
        {
            Character character = Instantiate(startingCharacter);
            character.transform.SetParent(playerCrewParent);
            newPlayer.Crew.Add(character);
            shipGrid.AddUnit(character, shipGrid.GetFreeCellForCharacterSpawn(HexCell.SpawnType.Player), true);
        }

        OnHumanPlayerCreated?.Invoke(newPlayer);
    }

    //Creates an AI controlled merchant player from a prefab and spawns a ship and adds it to the controller
    private void CreateMerchantPlayer()
    {
        Transform aiTransform = Instantiate(aiPrefab).transform;
        aiTransform.SetParent(shipTransform);

        Ship newShip = Instantiate(aiMerchantShip);
        newShip.transform.SetParent(aiTransform);

        terrainGrid.AddUnit(newShip, terrainGrid.GetRandomFreeHarbor(), HexDirectionExtension.ReturnRandomDirection(), false);

        Player newMerchantPlayer = new Player(newShip, false);
        MapTurnSystem.instance.AddPlayerToTurnOrder(newMerchantPlayer);
    }
}

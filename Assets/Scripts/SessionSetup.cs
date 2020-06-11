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
    public int shipCellCountX = 15, shipCellCountY = 12;

    [Header("Player setup")]
    public Ship playerStarterShip;
    public CrewSimulation playerCrewSimulation;
    public Character startingCharacter;

    [Header("AI setup")]
    public GameObject aiPrefab;
    public Ship aiMerchantShip;
    public int numberOfMerchants = 5;

    // Start is called before the first frame update
    void Start()
    {
        ConfirmSetup();
    }

    //Used to confirm the settings of the game session
    public void ConfirmSetup()
    {
        terrainGrid.CreateMap(mapCellCountX, mapCellCountY, true);
        shipGrid.CreateMap(shipCellCountX, shipCellCountY, true);

        terrainGrid.SetCameraBoundriesToMatchHexGrid();
        shipGrid.SetCameraBoundriesToMatchHexGrid();

        CreateHumanPlayer();

        for (int i = 0; i < numberOfMerchants; i++)
        {
            CreateMerchantPlayer();
        }

        TurnSystem.instance.DoFirstTurn();
    }

    //Creates a player from a prefab and spawns a ship and adds it to the controller
    private void CreateHumanPlayer()
    {
        Ship ship = Instantiate(playerStarterShip);
        ship.transform.SetParent(shipTransform);

        terrainGrid.AddShip(ship, terrainGrid.GetRandomFreeHarbor(), HexDirectionExtension.ReturnRandomDirection(), true);

        playerCrewSimulation.ship = ship;

        Player newPlayer = new Player(ship, true, playerCrewSimulation);
        TurnSystem.instance.AddPlayerToTurnOrder(newPlayer);

        Character character = Instantiate(startingCharacter);
        character.transform.SetParent(playerCrewParent);

        shipGrid.AddCharacter(character, shipGrid.GetRandomFreeCell(), true);
    }

    //Creates an AI controlled merchant player from a prefab and spawns a ship and adds it to the controller
    private void CreateMerchantPlayer()
    {
        Transform aiTransform = Instantiate(aiPrefab).transform;
        aiTransform.SetParent(shipTransform);

        Ship ship = Instantiate(aiMerchantShip);
        ship.transform.SetParent(aiTransform);

        terrainGrid.AddShip(ship, terrainGrid.GetRandomFreeHarbor(), HexDirectionExtension.ReturnRandomDirection(), true);

        Player newMerchantPlayer = new Player(ship, false);
        TurnSystem.instance.AddPlayerToTurnOrder(newMerchantPlayer);
    }
}

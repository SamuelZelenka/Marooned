using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionSetup : MonoBehaviour
{
    public HexGrid hexgrid;
    public Transform playersParentTransform;

    public int cellCountX = 20, cellCountY = 15;

    public int numberOfMerchants = 5;

    public GameObject playerPrefab;
    public GameObject aiPrefab;
    public Ship playerStarterShip;
    public Ship aiMerchantShip;

    // Start is called before the first frame update
    void Start()
    {
        ConfirmSetup();
    }

    //Used to confirm the settings of the game session
    public void ConfirmSetup()
    {
        hexgrid.CreateMap(cellCountX, cellCountY, true);
        CreateHumanPlayer();

        for (int i = 0; i < numberOfMerchants; i++)
        {
            CreateMerchantPlayer();
        }

        HexGridController.instance.DoFirstTurn();
    }

    //Creates a player from a prefab and spawns a ship and adds it to the controller
    private void CreateHumanPlayer()
    {
        Transform playerTransform = Instantiate(playerPrefab).transform;
        playerTransform.SetParent(playersParentTransform);

        Ship ship = Instantiate(playerStarterShip);
        ship.transform.SetParent(playerTransform);

        hexgrid.AddShip(ship, hexgrid.GetRandomFreeHarbor(), HexDirectionExtension.ReturnRandomDirection(), true);

        CrewSimulation crewSimulation = playerTransform.GetComponent<CrewSimulation>();
        crewSimulation.ship = ship;

        PlayerInput playerInput = playerTransform.GetComponent<PlayerInput>();
        playerInput.terrainGrid = hexgrid;

        Player newPlayer = new Player(ship, true, crewSimulation);
        HexGridController.instance.AddPlayerToTurnOrder(newPlayer);
    }

    //Creates an AI controlled merchant player from a prefab and spawns a ship and adds it to the controller
    private void CreateMerchantPlayer()
    {
        Transform playerTransform = Instantiate(aiPrefab).transform;
        playerTransform.SetParent(playersParentTransform);

        Ship ship = Instantiate(aiMerchantShip);
        ship.transform.SetParent(playerTransform);

        hexgrid.AddShip(ship, hexgrid.GetRandomFreeHarbor(), HexDirectionExtension.ReturnRandomDirection(), true);

        Player newMerchantPlayer = new Player(ship, false);
        HexGridController.instance.AddPlayerToTurnOrder(newMerchantPlayer);
    }
}

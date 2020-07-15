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
    public TerrainMap terrainMap;

    public int mapCellCountX = 20, mapCellCountY = 15;

    [Header("Player setup")]
    public Ship playerStarterShip;
    public BattleMap playerStartingGridMap;
    public CrewSimulation playerCrewSimulation;
    public Character[] startingCharacters;
    public CombatSystem combatSystem;

    [Header("AI setup")]

    public int numberOfMerchantRoutes = 5;

    // Start is called before the first frame update
    void Start() => ConfirmSetup();

    //Used to confirm the settings of the game session
    public void ConfirmSetup()
    {
        terrainGrid.CreateMap(mapCellCountX, mapCellCountY, true, true, true);
        terrainMap.Setup(terrainGrid, numberOfMerchantRoutes);
        shipGrid.Load(playerStartingGridMap, true);

        terrainGrid.SetCameraBoundriesToMatchHexGrid();
        shipGrid.SetCameraBoundriesToMatchHexGrid();

        CreateHumanPlayer();

        MapTurnSystem.instance.DoFirstTurn();
    }

    //Creates a player from a prefab and spawns a ship and adds it to the controller
    private void CreateHumanPlayer()
    {
        Ship newShip = Instantiate(playerStarterShip);
        newShip.transform.SetParent(shipTransform);

        terrainGrid.AddUnit(newShip, terrainMap.GetRandomFreeHarbor(), HexDirectionExtension.ReturnRandomDirection(), true);

        combatSystem.managementMap = playerStartingGridMap;

        Player newPlayer = new Player(newShip, true, playerCrewSimulation);
        MapTurnSystem.instance.AddPlayerToTurnOrder(newPlayer);

        for (int i = 0; i < startingCharacters.Length; i++)
        {
            Character character = Instantiate(startingCharacters[i]);
            character.transform.SetParent(playerCrewParent);
            newPlayer.Crew.Add(character);
            shipGrid.AddUnit(character, shipGrid.GetFreeCellForCharacterSpawn(HexCell.SpawnType.Player), true);
        }
        HexGridController.player = newPlayer;
    }

    
}

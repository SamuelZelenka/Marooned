using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [Header("References")]
    public HexGrid hexGrid;
    public Transform playerCharacterParent;
    public Transform enemyCharacterParent;

    public GameObject combatCanvas;
    public GameObject mapView;
    public GameObject combatView;
    public CombatTurnSystem turnSystem;

    Player humanPlayer;
    Ship playerShip;

    [Header("Setup")]
    [HideInInspector]
    public BattleMap managementMap;
    public BattleMap[] battleMaps;
    public Character[] debugEnemies;

    #region Setup References
    private void Awake() => SessionSetup.OnHumanPlayerCreated += DoSetup;

    private void DoSetup(Player humanPlayer)
    {
        this.humanPlayer = humanPlayer;
        playerShip = humanPlayer.Ship;
        //Unsubscribe
        SessionSetup.OnHumanPlayerCreated -= DoSetup;
    }
    #endregion

    public void StartCombat()
    {
        ChangeView(true);
        SetUpCombat(0);
    }

    private void SetUpCombat(int size)
    {
        hexGrid.Load(battleMaps[size], false);
        List<Character> allCharacters = new List<Character>();

        //Player characters
        allCharacters.AddRange(humanPlayer.Crew);

        //Enemy characters
        foreach (Character charactersToSpawn in debugEnemies)
        {
            //Instantiate enemies
            Character spawnedCharacter = Instantiate(charactersToSpawn);
            spawnedCharacter.transform.SetParent(enemyCharacterParent);

            spawnedCharacter.myGrid = hexGrid;

            //Add character to grid
            hexGrid.AddUnit(spawnedCharacter, hexGrid.GetFreeCellForCharacterSpawn(HexCell.SpawnType.AnyEnemy), false);

            //Add character to list of all characters involved in combat
            allCharacters.Add(spawnedCharacter);
        }

        turnSystem.SetupNewCombat(allCharacters);
        turnSystem.StartCombat();
    }

    public void EndCombat()
    {
        hexGrid.Load(managementMap, false);

        foreach (var item in humanPlayer.Crew)
        {
            //Add move characters back to their saved location
            item.Location = hexGrid.GetCell(item.SavedShipLocation.coordinates);
            item.SavedShipLocation = item.Location;
        }
        ChangeView(false);
    }

    private void ChangeView(bool showCombat)
    {
        combatCanvas.SetActive(showCombat);
        combatView.SetActive(showCombat);
        mapView.SetActive(!showCombat);
    }
}

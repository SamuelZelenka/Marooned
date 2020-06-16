using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [Header("References")]
    public HexGrid hexGrid;
    public Transform playerCharacterParent;
    public Transform enemyCharacterParent;

    [HideInInspector]
    public Ship playerShip;

    [Header("Setup")]
    [HideInInspector]
    public BattleMap managementMap;
    public BattleMap[] battleMaps;
    public Character[] debugEnemies;


    public void StartCombat()
    {
        SetUpCombat(0);
    }

    private void SetUpCombat(int size)
    {
        hexGrid.Load(battleMaps[size], false);

        foreach (Character charactersToSpawn in debugEnemies)
        {
            //Instantiate
            Character spawnedCharacter = Instantiate(charactersToSpawn);
            spawnedCharacter.transform.SetParent(enemyCharacterParent);

            spawnedCharacter.myGrid = hexGrid;

            //Add
            hexGrid.AddUnit(spawnedCharacter, hexGrid.GetFreeCellForCharacterSpawn(HexCell.SpawnType.AnyEnemy), false);
        }
    }

    public void EndCombat()
    {
        hexGrid.Load(managementMap, false);

        foreach (var item in playerShip.crew)
        {
            hexGrid.AddUnit(item, hexGrid.GetCell(item.SavedShipLocation.coordinates), true);
        }
    }
}

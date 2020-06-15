using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    public HexGrid hexGrid;

    public BattleMap managementMap;
    public BattleMap[] battleMaps;

    public Ship playerShip;

    public Character[] debugEnemies;

    public void StartCombat()
    {
        SetUpCombat(0);
    }

    private void SetUpCombat(int size)
    {
        hexGrid.Load(battleMaps[size]);

        foreach (var item in playerShip.crew)
        {
            hexGrid.AddCharacter(item, hexGrid.GetCell(item.Location.coordinates), true);
        }
    }

    public void EndCombat()
    {
        hexGrid.Load(managementMap);

        foreach (var item in playerShip.crew)
        {
            hexGrid.AddCharacter(item, hexGrid.GetCell(item.Location.coordinates), true);
        }
    }
}

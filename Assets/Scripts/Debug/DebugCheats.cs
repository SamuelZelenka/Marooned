using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCheats : MonoBehaviour
{
    [SerializeField] HexGrid hexGrid = null;
    [SerializeField] Character hexUnit = null;
    [SerializeField] HexCell hexCell = null;


    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKey(KeyCode.C))
            {
                hexGrid.AddCharacter(hexUnit, hexCell, true);
            }
            if(Input.GetKey(KeyCode.S))
            {
                hexGrid.ShowGameGrid(true);
            }
        }
    }
}

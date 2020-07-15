using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    Camera playerCamera;

    bool updatePathfinding = true;

    HexCell mouseHooverCell;
    HexCell MouseHooverCell
    {
        get => mouseHooverCell;
        set
        {
            mouseHooverCell = value;
            updatePathfinding = true;
        }
    }

    private void Start()
    {
        playerCamera = Camera.main; //Main camera, can be exchanged if needed
    }

    private void OnEnable()
    {
        HexCell.OnHexCellHoover += DoMouseHooverCellSelection;
    }

    private void OnDisable()
    {
        HexCell.OnHexCellHoover -= DoMouseHooverCellSelection;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (Input.anyKey)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                HexGridController.SelectedCell = MouseHooverCell;
            }
            if (Input.GetKey(KeyCode.Mouse1))
            {
                if (updatePathfinding)
                {
                    DoPathfinding(MouseHooverCell);
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            DoMove();
        }
    }

    void DoMouseHooverCellSelection(HexCell cell) => MouseHooverCell = cell;

    #region Movement
    void DoPathfinding(HexCell hooverCell)
    {
        Debug.Log("Doing pathfinding");
        HexUnit activeUnit;
        if (HexGridController.CurrentMode == HexGridController.GridMode.Map)
        {
            activeUnit = HexGridController.ActiveShip;
        }
        else
        {
            activeUnit = HexGridController.ActiveCharacter;
        }

        if (hooverCell && activeUnit.CanEnter(hooverCell))
        {
            Pathfinding.FindPath(activeUnit.Location, hooverCell, activeUnit, true);
        }
        else
        {
            Pathfinding.ClearPath();
        }
        updatePathfinding = false;
    }

    void DoMove()
    {
        HexUnit activeUnit;
        if (HexGridController.CurrentMode == HexGridController.GridMode.Map)
        {
            activeUnit = HexGridController.ActiveShip;
        }
        else
        {
            activeUnit = HexGridController.ActiveCharacter;
        }

        if (activeUnit)
        {
            if (Pathfinding.HasPath)
            {
                Debug.Log("Doing move");
                List<HexCell> reachablePathThisTurn = Pathfinding.GetReachablePath(activeUnit, out int cost);
                if (reachablePathThisTurn != null && reachablePathThisTurn.Count > 1) //An actual path, longer than the included start hex where the unit stands now
                {
                    StartCoroutine(activeUnit.Travel(reachablePathThisTurn));
                    Pathfinding.ClearPath();
                }
            }
            else
            {
                Debug.Log("No path");
            }
        }
    }
    #endregion
}

using UnityEngine;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour
{
    Camera playerCamera;
    public HexGrid terrainGrid;
    public HexGrid shipGrid;


    HexCell currentCell;
    HexUnit selectedUnit;

    private void Start()
    {
        playerCamera = Camera.main; //Main camera, can be exchanged if needed
    }

    private void Update()
    {
        //if (!EventSystem.current.CompareTag("UI"))
        //{
        HandleInput();
        //}
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            DoSelection();
        }
        else if (selectedUnit && !selectedUnit.IsMoving && selectedUnit.playerControlled)
        {
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                DoMove();
            }
            else if (Input.GetKey(KeyCode.Mouse1))
            {
                DoPathfinding();
            }
        }
    }

    bool UpdateCurrentCell()
    {
        HexCell cell = terrainGrid.gameObject.activeInHierarchy ? terrainGrid.GetCell() : shipGrid.GetCell();
        if (cell != currentCell)
        {
            currentCell = cell;
            return true;
        }
        return false;
    }

    void DoSelection()
    {
        UpdateCurrentCell();
        if (currentCell)
        {
            selectedUnit = currentCell.Unit;

            if (selectedUnit)
            {
                selectedUnit.pathfinding.ClearPath();
                Debug.Log("Selected unit");
            }
        }
    }

    void DoPathfinding()
    {
        if (UpdateCurrentCell())
        {
            if (currentCell && selectedUnit.CanMoveTo(currentCell))
            {
                selectedUnit.pathfinding.FindPath(selectedUnit.Location, currentCell, selectedUnit);
            }
            else
            {
                selectedUnit.pathfinding.ClearPath();
            }
        }
    }

    void DoMove()
    {
        if (selectedUnit.pathfinding.HasPath)
        {
            List<HexCell> reachablePathThisTurn = selectedUnit.pathfinding.GetReachablePath(selectedUnit, out int cost);
            if (reachablePathThisTurn != null && reachablePathThisTurn.Count > 1) //An actual path, longer than the included start hex where the unit stands now
            {
                StartCoroutine(selectedUnit.Travel(reachablePathThisTurn));
                selectedUnit.remainingMovementPoints -= cost;
                selectedUnit.pathfinding.ClearPath();
            }
        }
    }
}

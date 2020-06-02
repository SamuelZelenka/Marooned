using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    public HexGrid hexGrid;
    public Pathfinding playerPathfinding;

    HexCell currentCell;
    HexUnit selectedUnit;

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
        else if (selectedUnit && !selectedUnit.IsMoving)
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
        HexCell cell = hexGrid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
        if (cell != currentCell)
        {
            currentCell = cell;
            return true;
        }
        return false;
    }

    void DoSelection()
    {
        playerPathfinding.ClearPath();
        UpdateCurrentCell();
        if (currentCell)
        {
            selectedUnit = currentCell.Unit;

            if (selectedUnit)
            {
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
                playerPathfinding.FindPath(selectedUnit.Location, currentCell, selectedUnit);
            }
            else
            {
                playerPathfinding.ClearPath();
            }
        }
    }

    void DoMove()
    {
        if (playerPathfinding.HasPath)
        {
            selectedUnit.Travel(playerPathfinding.GetPath());

            playerPathfinding.ClearPath();
        }
    }
}

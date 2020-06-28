using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    Camera playerCamera;
    public HexGrid terrainGrid;
    public HexGrid shipGrid;
    public CombatSystem combatSystem;

    bool combatModeActive = false;
    bool updatePathfinding = true;

    HexCell selectedCell;
    HexCell SelectedCell
    {
        get => selectedCell;
        set
        {
            selectedCell = value;
            if (combatModeActive)
            {
                combatSystem.UseAbility(value);
            }
        }
    }
    HexUnit ActiveUnit
    {
        get;
        set;
    }
    HexUnit selectedUnit;
    HexUnit SelectedUnit
    {
        get => selectedUnit;
        set
        {
            selectedUnit = value;
            if (value != null && !combatModeActive && value.playerControlled)
            {
                ActiveUnit = value;
            }
        }
    }
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
        CombatSystem.OnCombatStart += StartCombatMode;
        CombatSystem.OnCombatEnd += EndCombatMode;
        CombatTurnSystem.OnTurnBegining += DoUnitSelection;
        HexCell.OnHexCellHoover += DoMouseHooverCellSelection;
    }

    private void OnDisable()
    {
        CombatSystem.OnCombatStart -= StartCombatMode;
        CombatSystem.OnCombatEnd -= EndCombatMode;
        CombatTurnSystem.OnTurnBegining -= DoUnitSelection;
        HexCell.OnHexCellHoover -= DoMouseHooverCellSelection;
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                SelectedCell = MouseHooverCell;
                SelectedUnit = SelectedCell.Unit;
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

    void StartCombatMode() => combatModeActive = true;
    void EndCombatMode() => combatModeActive = false;

    void DoUnitSelection(HexUnit unit) => ActiveUnit = unit;
    void DoMouseHooverCellSelection(HexCell cell) => MouseHooverCell = cell;

    //HexCell GetCellUnderMouse() => terrainGrid.gameObject.activeInHierarchy? terrainGrid.GetCell() : shipGrid.GetCell();

    #region Movement
    void DoPathfinding(HexCell hooverCell)
    {
        Debug.Log("Doing pathfinding");
        if (hooverCell && ActiveUnit.CanMoveTo(hooverCell))
        {
            Pathfinding.FindPath(ActiveUnit.Location, hooverCell, ActiveUnit, true);
        }
        else
        {
            Pathfinding.ClearPath();
        }
        updatePathfinding = false;
    }

    void DoMove()
    {
        if (ActiveUnit)
        {
            if (Pathfinding.HasPath)
            {
                Debug.Log("Doing move");
                List<HexCell> reachablePathThisTurn = Pathfinding.GetReachablePath(ActiveUnit, out int cost);
                if (reachablePathThisTurn != null && reachablePathThisTurn.Count > 1) //An actual path, longer than the included start hex where the unit stands now
                {
                    StartCoroutine(ActiveUnit.Travel(reachablePathThisTurn));
                    ActiveUnit.remainingMovementPoints -= cost;
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


    public Character GetActiveCharacter()
    {
        if (ActiveUnit is Character)
        {
            return ActiveUnit as Character;
        }
        else
        {
            return null;
        }
    }
}

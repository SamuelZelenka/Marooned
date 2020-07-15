using UnityEngine;

public class HexGridController : MonoBehaviour
{
    public enum GridMode { Map, Combat, Management }
    static GridMode currentMode;
    static HexCell selectedCell;
    static Character selectedCharacter;
    static Character activeCharacter;
    static Ship activeShip;
    public static Player player;

    public delegate void CellHandler(HexCell cell);
    public static event CellHandler OnCellSelected;
    public delegate void CharacterHandler(Character character);
    public static event CharacterHandler OnCharacterSelected;
    public static event CharacterHandler OnActiveCharacterChanged;
    public delegate void ShipHandler(Ship ship);
    public static event ShipHandler OnActiveShipChanged;

    public delegate void ModeHandler(GridMode mode);
    public static event ModeHandler OnModeChangedTo;

    public static GridMode CurrentMode
    {
        get => currentMode;
        set
        {
            currentMode = value;
            OnModeChangedTo?.Invoke(value);
        }
    }

    public static HexCell SelectedCell
    {
        get => selectedCell;
        set
        {
            selectedCell = value;
            if (selectedCell)
            {
                SelectedCharacter = value.Unit as Character;
            }
            if (currentMode == GridMode.Management) //Exception to regular rule on turnorders. In management mode the active unit can be selected just by selecting a cell
            {
                if (ActiveCharacter)
                {
                    ActiveCharacter.EndTurn();
                }
                ActiveCharacter = SelectedCharacter;
                if (ActiveCharacter)
                {
                    ActiveCharacter.StartNewTurn(); 
                }
            }
            OnCellSelected?.Invoke(value);
        }
    }
    public static Character ActiveCharacter
    {
        get => activeCharacter;
        set
        {
            activeCharacter = value;
            OnActiveCharacterChanged?.Invoke(value);
        }
    }
    public static Ship ActiveShip
    {
        get => activeShip;
        set
        {
            if (ActiveShip)
            {
                ActiveShip.ShowUnitActive(false);
            }
            activeShip = value;
            if (ActiveShip)
            {
                ActiveShip.ShowUnitActive(true);
            }
            OnActiveShipChanged?.Invoke(value);
        }
    }

    public static Character SelectedCharacter
    {
        private set
        {
            selectedCharacter = value;
            OnCharacterSelected?.Invoke(value);
        }
        get => selectedCharacter;
    }

    public void StartMapMode() => CurrentMode = GridMode.Map;
    public void StartManagementMode() => CurrentMode = GridMode.Management;
    public void StartCombatMode() => CurrentMode = GridMode.Combat;
}
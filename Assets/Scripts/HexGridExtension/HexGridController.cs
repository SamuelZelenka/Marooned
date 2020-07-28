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
    public static HexGrid worldGrid;
    public static HexGrid playerShipGrid;
    public static Transform playerCrewParent = null;

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
                ActiveCharacter = SelectedCharacter;
            }
            OnCellSelected?.Invoke(value);
        }
    }
    public static Character SelectedCharacter
    {
        private set
        {
            if (value == selectedCharacter)
            {
                return;
            }
            selectedCharacter = value;
            OnCharacterSelected?.Invoke(value);
        }
        get => selectedCharacter;
    }
    public static Character ActiveCharacter
    {
        get => activeCharacter;
        set
        {
            if (value == ActiveCharacter)
            {
                return;
            }
            if (ActiveCharacter)
            {
                ActiveCharacter.MakeUnitInactive();
            }
            activeCharacter = value;
            if (ActiveCharacter)
            {
                ActiveCharacter.MakeUnitActive();
            }
            OnActiveCharacterChanged?.Invoke(value);
        }
    }
    public static Ship ActiveShip
    {
        get => activeShip;
        set
        {
            if (value == activeShip)
            {
                return;
            }
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

    public static void SpawnCharacterForPlayerCrew(Character character)
    {
        Character spawnedCharacter = Instantiate(character);
        spawnedCharacter.transform.SetParent(playerCrewParent);
        player.Crew.Add(spawnedCharacter);
        HexCell validCell = playerShipGrid.Cells.ReturnRandomElementWithCondition((c) => c.IsFree == true, (c) => c.TypeOfSpawnPos == HexCell.SpawnType.Player);
        playerShipGrid.AddUnit(spawnedCharacter, validCell, true);
    }

    public void StartMapMode() => CurrentMode = GridMode.Map;
    public void StartManagementMode() => CurrentMode = GridMode.Management;
    public void StartCombatMode() => CurrentMode = GridMode.Combat;
}
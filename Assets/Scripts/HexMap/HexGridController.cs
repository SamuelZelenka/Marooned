public static class HexGridController
{
    public enum GridMode { Map, Combat, Management}
    public static GridMode currentMode; 
    static HexCell selectedCell;
    static Character activeCharacter;
    static Ship activeShip;

    public delegate void CellHandler(HexCell cell);
    public static CellHandler OnCellSelected;

    public static HexCell SelectedCell 
    { 
        get => selectedCell; 
        set
        {
            selectedCell = value;
            OnCellSelected?.Invoke(value);
        }
    }
    public static Character ActiveCharacter 
    {
        get => activeCharacter;
        set { activeCharacter = value; }
    }
    public static Ship ActiveShip 
    {
        get => activeShip;
        set { activeShip = value; }
    }


    public static Character SelectedCharacter
    {
        get
        {
            if (selectedCell && selectedCell.Unit is Character)
            {
                return selectedCell.Unit as Character;
            }
            else
            {
                return null;
            }
        }
    }
}
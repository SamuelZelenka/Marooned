public static class HexGridController
{
    public enum GridMode { Map, Combat, Management}
    public static GridMode currentMode; 
    public static HexCell selectedCell;
    public static Character activeCharacter;
    public static Ship activeShip;

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
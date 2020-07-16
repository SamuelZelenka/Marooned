using System.Collections.Generic;

public class Landmass
{
    public List<HexCell> landCells = new List<HexCell>();
    public List<HexCell> GetShores()
    {
        List<HexCell> shores = new List<HexCell>();
        foreach (var item in landCells)
        {
            if (item.IsShore)
            {
                shores.Add(item);
            }
        }
        return shores;
    }
    public HexCell harbor;
}

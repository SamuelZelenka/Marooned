using UnityEngine;
using UnityEditor;

public class SaveLoadBattleMapMenu : MonoBehaviour
{
    public HexGrid hexGrid;

    public BattleMap selectedMap;

    public string folderName = "ZoomedMaps";

    string mapName = "New Battle Map";

    public void SetMapName(string name)
    {
        mapName = name;
    }

    public void Save()
    {
        BattleMap map;
        if (selectedMap == null)
        {
            map = Utility.CreateAsset<BattleMap>(folderName, mapName);
        }
        else
        {
            map = selectedMap;
        }

        map.Save(hexGrid.CellCountX, hexGrid.CellCountY, hexGrid.Save());

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Map Saved");
    }

    public void Load()
    {
        if (selectedMap)
        {
            hexGrid.Load(selectedMap);
        }
        hexGrid.ShowEditGrid(true);
        Debug.Log("Map Loaded");
    }
}

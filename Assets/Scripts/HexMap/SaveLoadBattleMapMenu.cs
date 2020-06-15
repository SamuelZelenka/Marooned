using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SaveLoadBattleMapMenu : MonoBehaviour
{
    public HexGrid hexGrid;

    public BattleMap selectedMap;
    public Text selectedMapTextInfo;

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

        UnityEditor.EditorUtility.SetDirty(map);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Map Saved");
    }

    private void Update()
    {
        selectedMapTextInfo.text = selectedMap != null ? selectedMap.name + " - is selected for loading/save=overwriting" : "No map selected for loading/overwriting";
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

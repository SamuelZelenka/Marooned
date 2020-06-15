using UnityEngine;
using UnityEditor;

public class SaveLoadBattleMapMenu : MonoBehaviour
{
    //public bool saveMode;
    public HexGrid hexGrid;

    public BattleMap selectedMap;

    //public void SaveLoad()
    //{
    //    if (saveMode)
    //    {
    //        Debug.Log("Saving map");
    //        Save();
    //    }
    //    else
    //    {
    //        Debug.Log("Loading map");
    //        Load();
    //    }
    //    //nameInput.text = "Enter a new name or choose an existing map";
    //    //Close();
    //}

    public void Save()
    {
        BattleMap map;
        if (selectedMap == null)
        {
            map = Utility.CreateAsset<BattleMap>();
        }
        else
        {
            map = selectedMap;
        }

        map.Save(hexGrid.CellCountX, hexGrid.CellCountY, hexGrid.Save());

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public void Load()
    {
        if (selectedMap)
        {
            hexGrid.Load(selectedMap);
        }
        hexGrid.ShowEditGrid(true);
    }
}

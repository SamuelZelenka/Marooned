using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public List<Setting<bool>> boolSettingPairs = new List<Setting<bool>>();

    #region Singleton
    private Settings() { }

        private static Settings instance;

        public static Settings GetInstance()
        {
            if (instance == null)
            {
                instance = new Settings();
            }
            return instance;
        }
    #endregion
    private void Start()
    {
       // mouseEdgeDetection = new BoolSetting("MouseEdgeDetection", mouseEdgeToggle.isOn);
    }
    #region Gameplay



    #endregion
    #region Controls
    public void ToggleMouseEdgeDetection() 
    {
        // PlayerPrefs.SetInt(boolSetting.pair.Key, BoolToInt(boolSetting.pair.Value)); = new KeyValuePair<string, bool>("MouseEdgeDetection", mouseEdgeToggle.isOn);
    }
    #endregion
    #region Audio



    #endregion
    #region Graphics



    #endregion
    public void SaveSettings()
    {
        //Save all bool settings
        foreach (Setting<bool> boolSetting in boolSettingPairs)
        {
            
        }
    }
    public void LoadSettings()
    {
        for (int i = 0; i < boolSettingPairs.Count; i++)
        {
            boolSettingPairs[i].pair = new KeyValuePair<string, bool>(boolSettingPairs[i].pair.Key, IntToBool(PlayerPrefs.GetInt(boolSettingPairs[i].pair.Key)));
        }
    }
    private int BoolToInt(bool inBool)
    {
        if (inBool) return 1;
        else return 0;
    }
    private bool IntToBool(int inInt)
    {
        if (inInt == 0) return false;
        else return true;
    }
}
public abstract class Setting<T>
{
    public KeyValuePair<string, T> pair;
    public abstract T GetValue();
}

public class BoolSetting : Setting<bool>
{
    bool value;
    public BoolSetting(string key, bool value)
    {
        Settings.GetInstance().boolSettingPairs.Add(this);
        this.value = value;
    }
    public override bool GetValue()
    {
        return value;
    }
}
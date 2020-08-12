using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSystem : MonoBehaviour
{
    #region NewGame
    SetupData setupData = new SetupData();

    public MapSize[] mapSizes = new MapSize[] { new MapSize("Small", 20, 15), new MapSize("Medium", 30, 20), new MapSize("Large", 40, 25) };
    public DifficultySettings[] difficultySettings = new DifficultySettings[] { new DifficultySettings("Easy", 600, 2, 6), new DifficultySettings("Medium", 500, 2, 8), new DifficultySettings("Hard", 400, 3, 10) };

    public MapSize SelectedMapSize
    {
        get;
        private set;
    }

    public SettingsChooser difficulty;
    public SettingsChooser mapSize;

    private void Start()
    {
        difficulty.OnIndexChanged += SetDifficultyLevel;
        mapSize.OnIndexChanged += SetMapSize;

        foreach (var item in difficultySettings)
        {
            difficulty.AddOption(item.name);
        }

        foreach (var item in mapSizes)
        {
            mapSize.AddOption(item.name);
        }
    }

    public void SetDifficultyLevel(int index) => setupData.difficultySettings = difficultySettings[index];
    public void SetSeed(string value) => setupData.stringSeed = value;
    public void SetMapSize(int index)
    {
        SelectedMapSize = mapSizes[index];
        SetWorldXSize(SelectedMapSize.xSize);
        SetWorldYSize(SelectedMapSize.ySize);
    }
    private void SetWorldXSize(int value) => setupData.mapCellCountX = value;
    private void SetWorldYSize(int value) => setupData.mapCellCountY = value;
    public void SetNumberOfRoutes(float value) => setupData.routes = Mathf.RoundToInt(value);
    public void SetIslandSpawnChance(float value) => setupData.newLandmassChance = value;
    public void SetLandDensity(float value) => setupData.landByLandChance = value;
    public void SetLandmassMaxSize(float value) => setupData.landMassMaxSize = Mathf.RoundToInt(value);
    #endregion
}

using UnityEngine;
[System.Serializable]

public class Level
{
    //public static readonly string[] BOUNTYNAMES = new string[] { "Landlubber", "Cannonfodder", "Freebooter", "Seadog", "Skirmisher", "Bootyfinder", "Seasoned", "First mate", "Expert", "Legend" };
    public const int BOUNTYPERLEVEL = 100;
    public const int FIRSTLEVELUPXPREQUIREMENT = 10;
    public const int XPREQUIREMENTINCREASEFACTOR = 2;

    public delegate void LevelHandler();
    public event LevelHandler OnLevelChanged;

    private int currentLevel;
    public int CurrentLevel
    {
        get => currentLevel;
        private set
        {
            currentLevel = value;
            OnLevelChanged?.Invoke();
        }
    }
    public int Bounty
    {
        get => CurrentLevel * BOUNTYPERLEVEL;
    }
    public int XPLevelUpRequirement
    {
        get;
        set;
    }
    public int XP
    {
        get;
        set;
    }

    public Level(int startLevel)
    {
        this.CurrentLevel = startLevel;
        int xpReq = FIRSTLEVELUPXPREQUIREMENT;
        int levelsAboveFirst = startLevel - 1;
        for (int i = 0; i < levelsAboveFirst; i++)
        {
            xpReq *= XPREQUIREMENTINCREASEFACTOR;
        }
        XPLevelUpRequirement = xpReq;
    }

    public void IncreaseLevel(int increase) => CurrentLevel += increase;

    public override string ToString()
    {
        return $"£{Bounty}";
    }
}
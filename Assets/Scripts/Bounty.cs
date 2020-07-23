using UnityEngine;
[System.Serializable]

public class Bounty
{
    public static readonly string[] BOUNTYNAMES = new string[] { "Landlubber", "Cannonfodder", "Freebooter", "Seadog", "Skirmisher", "Bootyfinder", "Seasoned", "First mate", "Expert", "Legend" };
    public static readonly int[] BOUNTYLEVELVALUES = new int[] { 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };

    public const int MINBOUNTYLEVEL = 1;
    public const int MAXBOUNTYLEVEL = 10;

    public delegate void BountyHandler();
    public event BountyHandler OnBountyChanged;

    public string statName;
    private int currentValue;
    public int CurrentValue
    {
        get => currentValue;
        private set
        {
            currentValue = Mathf.Clamp(value, MINBOUNTYLEVEL, MAXBOUNTYLEVEL);
            OnBountyChanged?.Invoke();
        }
    }

    public void IncreaseStat(int increase) => CurrentValue += increase;
    public void DecreaseStat(int decrease) => CurrentValue -= decrease;

    public Bounty(string statName, int currentValue)
    {
        this.statName = statName;
        this.currentValue = currentValue;
    }
    public override string ToString()
    {
        return CurrentValue.ToString();
    }
}
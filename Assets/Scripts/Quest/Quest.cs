using UnityEngine;

public abstract class Quest : ScriptableObject
{
    public string title;
    public string description;

    public string messageOnActivated;
    public string messageOnCompleted;
    public string messageOnFailed;

    public abstract void QuestStarted();
}

[CreateAssetMenu(menuName = "ScriptableObject/LocationQuest", fileName = "New Location Quest")]
public class LocationQuest : Quest
{
    public HexCell cellToReach;
    public Player player;

    public void Setup(Player player, HexCell cellToReach)
    {
        this.player = player;
        this.cellToReach = cellToReach;
    }
    public override void QuestStarted()
    {
        player.Ship.OnUnitMoved += CheckIfReached;
    }

    private void CheckIfReached(HexUnit unitMoved)
    {
        if (unitMoved.Location == cellToReach)
        {
            player.PlayerData.RemoveQuest(this, true);
        }
    }
}

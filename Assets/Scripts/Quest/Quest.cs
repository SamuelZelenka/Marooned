using UnityEngine;

public abstract class Quest : ScriptableObject
{
    public string title;
    public string description;

    public string messageOnActivated;
    public string messageOnCompleted;
    public string messageOnFailed;

    public delegate void QuestHandler();
    public QuestHandler OnQuestCompleted;

    protected Player player;
    protected void Setup(Player player) => this.player = player;

    public abstract void QuestStarted();
    protected abstract void CompleteQuest();
}

[CreateAssetMenu(menuName = "ScriptableObject/Quest/Travel Quest", fileName = "New Travel Quest")]
public class TravelQuest : Quest
{
    public HexCell cellToReach;

    public void Setup(Player player, HexCell cellToReach)
    {
        base.Setup(player);
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
            CompleteQuest();
        }
    }

    protected override void CompleteQuest()
    {
        //Rewards


        OnQuestCompleted?.Invoke();
    }
}


[CreateAssetMenu(menuName = "ScriptableObject/Quest/POIQuest", fileName = "New POI Quest")]
public class POIQuest : Quest
{
    PointOfInterest pointOfInterest;

    public void Setup(Player player, PointOfInterest pointOfInterest)
    {
        base.Setup(player);
        this.pointOfInterest = pointOfInterest;
    }

    public override void QuestStarted()
    {
        pointOfInterest.OnInteractedWith += CompleteQuest;
    }

    private void CompleteQuest(PointOfInterest pointOfInterest) => CompleteQuest();

    protected override void CompleteQuest()
    {
        //Rewards


        OnQuestCompleted?.Invoke();
    }
}
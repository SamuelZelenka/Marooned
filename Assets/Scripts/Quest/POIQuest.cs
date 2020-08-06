using UnityEngine;

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

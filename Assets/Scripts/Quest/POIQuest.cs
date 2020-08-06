using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Quest/POIQuest", fileName = "New POI Quest")]
public class POIQuest : Quest
{
    public override void Setup(Player player, PointOfInterest poi)
    {
        base.Setup(player, poi);
        poi.OnInteractedWith += CompleteQuest;
    }

    private void CompleteQuest(PointOfInterest pointOfInterest) => CompleteQuest();

    protected override void CompleteQuest()
    {
        base.CompleteQuest();
        //Rewards

        OnQuestCompleted?.Invoke();
        poi.OnInteractedWith -= CompleteQuest;
    }
}

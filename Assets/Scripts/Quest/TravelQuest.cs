using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Quest/Travel Quest", fileName = "New Travel Quest")]
public class TravelQuest : Quest
{
    HexCell cellToReach;

    public override void Setup(Player player, PointOfInterest poi)
    {
        base.Setup(player, poi);
        this.cellToReach = poi.Hexcell;
        player.Ship.OnUnitMoved += CheckIfReached;
    }


    private void CheckIfReached(HexUnit unitMoved)
    {
        if (unitMoved.Location == cellToReach)
        {
            CompleteQuest();
        }
    }

    protected override void CompleteQuest()
    {
        base.CompleteQuest();
        //Rewards

        OnQuestCompleted?.Invoke();
    }
}
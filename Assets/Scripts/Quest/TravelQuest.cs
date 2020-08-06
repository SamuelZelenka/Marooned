using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Quest/Travel Quest", fileName = "New Travel Quest")]
public class TravelQuest : Quest
{
    public HexCell cellToReach;

    public void Setup(Player player, HexCell cellToReach, string locationName)
    {
        base.Setup(player);
        this.cellToReach = cellToReach;
        SetTravelPointName(locationName);
    }

    void SetTravelPointName(string locationName) => description = Utility.ReplaceWordInString(description, "NAME", locationName);

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
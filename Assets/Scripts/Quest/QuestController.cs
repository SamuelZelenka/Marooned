using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    [SerializeField] TravelQuest startingQuest = null;
    [SerializeField] WorldController worldController = null;
    Player player;

    [SerializeField] List<Quest> mainQuests = new List<Quest>();

    private void Awake()
    {
        SessionSetup.OnPlayerCreated += SetPlayer;
    }

    void SetPlayer(Player player) => this.player = player;

    public void GiveStartingQuest()
    {
        startingQuest.Setup(player, worldController.HarborCells[0], worldController.Harbors[0].name);
        startingQuest.OnQuestCompleted += GiveMainQuest;
        player.PlayerData.AddQuest(startingQuest);
    }

    private void GiveMainQuest()
    {
        if (mainQuests.Count > player.PlayerData.mainQuestIndex)
        {
            Quest questToGive = mainQuests[player.PlayerData.mainQuestIndex];

            player.PlayerData.AddQuest(questToGive);
        }

        else
            Debug.Log("Finished all quests");
    }
}

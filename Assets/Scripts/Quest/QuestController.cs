using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    [SerializeField] WorldController worldController = null;
    Player player;

    [SerializeField] List<Quest> mainQuests = new List<Quest>();

    private void Awake()
    {
        SessionSetup.OnPlayerCreated += SetPlayer;
    }

    void SetPlayer(Player player) => this.player = player;

    public void StartFirstMainQuest()
    {
        player.PlayerData.mainQuestIndex = 0;
        GiveMainQuest();
    }

    private void GiveMainQuest()
    {
        if (mainQuests.Count > player.PlayerData.mainQuestIndex)
        {
            Quest questToGive = mainQuests[player.PlayerData.mainQuestIndex];
            PointOfInterest poi = null;
            switch (questToGive.Request)
            {
                case Quest.POIRequest.StartingHarbor:
                    poi = worldController.Harbors[0];
                    break;
                case Quest.POIRequest.Stronghold:
                    poi = worldController.Strongholds[0];
                    break;
            }
            questToGive.Setup(player, poi);
            questToGive.OnQuestCompleted += MainQuestCompleted;
            player.PlayerData.AddQuest(questToGive);
        }

        else
            Debug.Log("Finished all quests");
    }

    private void MainQuestCompleted()
    {
        player.PlayerData.mainQuestIndex++;
        GiveMainQuest();
    }
}

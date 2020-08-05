using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    [SerializeField] LocationQuest startingQuest = null;
    [SerializeField] WorldController worldController = null;
    Player player;

    [SerializeField] List<Quest> mainQuests = new List<Quest>(); 

    private void Awake()
    {
        SessionSetup.OnPlayerCreated += SetPlayer;
    }

    void SetPlayer(Player player) => this.player = player;

    public void GiveFirstQuest()
    {
        startingQuest.Setup(player, worldController.HarborCells[0]);
        player.PlayerData.AddQuest(startingQuest);
    }
}

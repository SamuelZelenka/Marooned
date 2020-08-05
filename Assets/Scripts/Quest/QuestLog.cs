using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLog : MonoBehaviour
{
    [SerializeField] QuestLogElement questPrefab = null;
    List<QuestLogElement> questsInLog = new List<QuestLogElement>();
    [SerializeField] float yPadding = 50;

    [SerializeField] NotificationViewer notificationViewer = null;

    Player player;

    private void Awake()
    {
        SessionSetup.OnPlayerCreated += SetPlayer;
    }

    void SetPlayer(Player player)
    {
        this.player = player;
        SubscribeQuestUpdates();
    }


    void SubscribeQuestUpdates()
    {
        player.PlayerData.OnQuestAdded += AddQuest;
        player.PlayerData.OnQuestCompleted += CompleteQuest;
    }

    void AddQuest(Quest newQuest)
    {
        //Spawn questlogelement
        QuestLogElement newElement = GameObject.Instantiate(questPrefab, this.transform);
        newElement.Setup(newQuest);
        questsInLog.Add(newElement);

        notificationViewer.ShowMessage("New Quest Added", newQuest.title);

        UpdateQuestLog();
    }



    void CompleteQuest(Quest completedQuest)
    {
        QuestLogElement logElement = RemoveQuest(completedQuest);

        //Display completed quest text
        notificationViewer.ShowMessage("Quest Completed", completedQuest.title);


        //TEMP
        GameObject.Destroy(logElement.gameObject, 0.5f);

        UpdateQuestLog();
    }

    void FailQuest(Quest failedQuest)
    {
        QuestLogElement logElement = RemoveQuest(failedQuest);

        //Display failed quest text
        notificationViewer.ShowMessage("Quest Failed", failedQuest.title);

        //TEMP
        GameObject.Destroy(logElement.gameObject, 0.5f);

        UpdateQuestLog();
    }

    QuestLogElement RemoveQuest(Quest questToRemove)
    {
        foreach (var element in questsInLog)
        {
            if (element.Quest == questToRemove)
            {
                questsInLog.Remove(element);
                return element;
            }
        }
        Debug.LogError("Quest log element not found");
        return null;
    }

    void UpdateQuestLog()
    {
        float yPos = yPadding;
        //Sort questlogelements
        for (int i = 0; i < questsInLog.Count; i++)
        {
            questsInLog[i].rect.anchoredPosition = new Vector3(0, -yPos);
            yPos += questsInLog[i].rect.sizeDelta.y;
        }
    }
}

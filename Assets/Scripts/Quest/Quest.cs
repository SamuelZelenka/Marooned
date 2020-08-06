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
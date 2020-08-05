using System.Collections.Generic;

public enum ResourceType { Wool, Tobacco, Coffee, Silk, Ores, MAX = 5 }

[System.Serializable]
public class PlayerData
{
    public delegate void DataHandler();
    public DataHandler OnGoldChanged;
    public DataHandler OnBountyChanged;
    public delegate void QuestHandler(Quest quest);
    public QuestHandler OnQuestAdded;
    public QuestHandler OnQuestCompleted;
    public QuestHandler OnQuestFailed;

    public List<CharacterData> AliveCharacters { get; private set; } = new List<CharacterData>();
    public List<CharacterData> DeadCharacters { get; private set; } = new List<CharacterData>();
    public ResourceInventory Resources { get; private set; } = new ResourceInventory();
    int gold;
    public int Gold
    {
        get { return gold; }
        set
        {
            gold = value;
            OnGoldChanged?.Invoke();
        }
    }


    int previousBountyChange;
    public int PreviousBountyChange
    {
        get => previousBountyChange;
        set
        {
            previousBountyChange = value;
            OnBountyChanged?.Invoke();
        }
    }

    int nextBountyChange;
    public int NextBountyChange
    {
        get => nextBountyChange;
        set
        {
            nextBountyChange = value;
            OnBountyChanged?.Invoke();
        }
    }

    public int Bounty
    {
        get
        {
            int combinedValue = 0;
            foreach (var item in AliveCharacters)
            {
                combinedValue += item.BountyLevel.Bounty;
            }
            return combinedValue;
        }
    }

    List<Quest> activeQuests = new List<Quest>();
    List<Quest> completedQuests = new List<Quest>();
    List<Quest> failedQuests = new List<Quest>();
    public int mainQuestIndex = 0;

    public void AddQuest(Quest quest)
    {
        activeQuests.Add(quest);
        quest.QuestStarted();
        OnQuestAdded?.Invoke(quest);
    }

    public void RemoveQuest(Quest quest, bool completed)
    {
        activeQuests.Remove(quest);
        if (completed)
        {
            completedQuests.Add(quest);
            OnQuestCompleted?.Invoke(quest);
        }
        else
        {
            failedQuests.Add(quest);
            OnQuestFailed?.Invoke(quest);
        }
    }
}

[System.Serializable]
public class ResourceInventory
{
    public const int WOOLDEFAULTVALUE = 1;
    public const int TOBACCODEFAULTVALUE = 2;
    public const int COFFEEDEFAULTVALUE = 3;
    public const int SILKDEFAULTVALUE = 5;
    public const int ORESDEFAULTVALUE = 10;

    public static int GetDefaultValue(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Wool:
                return WOOLDEFAULTVALUE;
            case ResourceType.Tobacco:
                return TOBACCODEFAULTVALUE;
            case ResourceType.Coffee:
                return COFFEEDEFAULTVALUE;
            case ResourceType.Silk:
                return SILKDEFAULTVALUE;
            case ResourceType.Ores:
                return ORESDEFAULTVALUE;
        }
        return int.MinValue;
    }
    public Resource Wool { get; set; } = new Resource("Wool", 5);
    public Resource Tobacco { get; set; } = new Resource("Tobacco", 2);
    public Resource Coffee { get; set; } = new Resource("Coffee", 0);
    public Resource Silk { get; set; } = new Resource("Silk", 0);
    public Resource Ores { get; set; } = new Resource("Ores", 0);
    public Resource GetResource(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Wool:
                return Wool;
            case ResourceType.Tobacco:
                return Tobacco;
            case ResourceType.Coffee:
                return Coffee;
            case ResourceType.Silk:
                return Silk;
            case ResourceType.Ores:
                return Ores;
        }
        return null;
    }
    public int maxTonnage = 60;
}

using System.Collections.Generic;

public enum ResourceType { Wool, Tobacco, Coffee, Silk, Ores, MAX = 5 }

[System.Serializable]
public class PlayerData
{
    public const int MAXBOUNTYLEVEL = 100;

    public delegate void DataHandler();
    public DataHandler OnBountyChanged;
    public DataHandler OnGoldChanged;

    public List<CharacterData> AliveCharacters { get; private set; }
    public List<CharacterData> DeadCharacters { get; private set; }
    public ResourceInventory Resources { get; private set; } = new ResourceInventory();
    int gold;
    public int Gold {
        get { return gold; }
        set
        {
            OnGoldChanged?.Invoke();
            gold = value;
        }
    }

    int bountyLevel;
    public int BountyLevel
    {
        get { return bountyLevel; }
        set {
            OnBountyChanged?.Invoke();
            bountyLevel = value;
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

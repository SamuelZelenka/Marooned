using System.Collections.Generic;

public enum ResourceType { Wool, Tobacco, Coffee, Silk, Ores, MAX = 5 }

[System.Serializable]
public class PlayerData
{
    public const int MAXBOUNTYLEVEL = 100;

    public delegate void BountyLevelHandler();
    public BountyLevelHandler OnBountyChanged;

    List<CharacterData> characters;
    public ShipData ShipData { get; private set; } = new ShipData();
    public int Gold { get; set; }

    int bountyLevel = 12;
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
public class ShipData
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
    public Resource WoolResource { get; set; } = new Resource("Wool", 5);
    public Resource TobaccoResource { get; set; } = new Resource("Tobacco", 2);
    public Resource CoffeeResource { get; set; } = new Resource("Coffee", 0);
    public Resource SilkResource { get; set; } = new Resource("Silk", 0);
    public Resource OresResource { get; set; } = new Resource("Ores", 0);
    public Resource GetResource(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Wool:
                return WoolResource;
            case ResourceType.Tobacco:
                return TobaccoResource;
            case ResourceType.Coffee:
                return CoffeeResource;
            case ResourceType.Silk:
                return SilkResource;
            case ResourceType.Ores:
                return OresResource;
        }
        return null;
    }
    public int maxTonnage = 60;
    public class Resource
    {
        public delegate void ResourceHandler(Resource resource);
        public ResourceHandler OnResourceChanged;

        string name;
        int value;
        public int Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnResourceChanged?.Invoke(this);
            }
        }
        public Resource(string name, int value)
        {
            this.name = name;
            this.value = value;
        }
        public override string ToString()
        {
            return name;
        }
    }
}
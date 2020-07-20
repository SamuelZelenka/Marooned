using System.Collections;
using System.Collections.Generic;

public enum ResourceType { Wool, Tobacco, Coffee, Silk, Ores, MAX = 5 }

[System.Serializable]
public class PlayerData
{
    List<CharacterData> characters;
    public ShipData ShipData { get; private set; } = new ShipData();
    public int Gold { get; set; }
}

[System.Serializable]
public class ShipData
{
    public const int WOOLDEFAULTVALUE = 1;
    public const int TOBACCODEFAULTVALUE = 2;
    public const int COFFEEDEFAULTVALUE = 3;
    public const int SILKDEFAULTVALUE = 5;
    public const int ORESDEFAULTVALUE = 10;


    public Resource Wool { get; private set; } = new Resource("Wool", 5);
    public Resource Tobacco { get; private set; } = new Resource("Tobacco", 2);
    public Resource Coffee { get; private set; } = new Resource("Coffee", 0);
    public Resource Silk { get; private set; } = new Resource("Silk", 0);
    public Resource Ores { get; private set; } = new Resource("Ores", 0);
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

    public class Resource
    {
        public string resourceName;
        public int value;

        public Resource(string resourceName, int value)
        {
            this.resourceName = resourceName;
            this.value = value;
        }
    }
}

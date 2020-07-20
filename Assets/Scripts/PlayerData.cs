using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    List<CharacterData> characters;
    ShipData shipData;
    int gold;
}

[System.Serializable]
public class ShipData
{
    public Resource Wool { get; private set; } = new Resource("Wool", 0);
    public Resource Tobacco { get; private set; } = new Resource("Tobacco", 0);
    public Resource Coffee { get; private set; } = new Resource("Coffee", 0);
    public Resource Silk { get; private set; } = new Resource("Silk", 0);
    public Resource Ores { get; private set; } = new Resource("Ores", 0);
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

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

    public Resource WoolResource { get; set; } = new Wool(5);
    public Resource TobaccoResource { get; set; } = new Tobacco(2);
    public Resource CoffeeResource { get; set; } = new Coffee(0);
    public Resource SilkResource { get; set; } = new Silk(0);
    public Resource OresResource { get; set; } = new Ores(0);
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

    public abstract class Resource
    {
        public delegate void ResourceHandler();

        public static ResourceHandler OnWoolChanged;
        public static ResourceHandler OnTobaccoChanged;
        public static ResourceHandler OnCoffeeChanged;
        public static ResourceHandler OnSilkChanged;
        public static ResourceHandler OnOresChanged;

        protected int value;
        public abstract int Value { get; set; }
        public abstract void OnValueChanged();
    }
    public class Wool : Resource
    {
        public override int Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnValueChanged();
            }
        }
        public Wool(int value)
        {
            this.value = value;
        }
        public override void OnValueChanged()
        {
            OnWoolChanged?.Invoke();
        }
    }
   
    public class Tobacco : Resource
    {
        public override int Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnValueChanged();
            }
        }
        public Tobacco(int value)
        {
            this.value = value;
        }
        public override void OnValueChanged()
        {
            OnTobaccoChanged?.Invoke();
        }
    }
    public class Coffee : Resource
    {
        public override int Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnValueChanged();
            }
        }
        public Coffee(int value)
        {
            this.value = value;
        }
        public override void OnValueChanged()
        {
            OnCoffeeChanged?.Invoke();
        }
    }
    public class Silk : Resource
    {
        public override int Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnValueChanged();
            }
        }
        public Silk(int value)
        {
            this.value = value;
        }
        public override void OnValueChanged()
        {
            OnSilkChanged?.Invoke();
        }
    }
    public class Ores : Resource
    {
        public override int Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnValueChanged();
            }
        }
        public Ores(int value)
        {
            this.value = value;
        }
        public override void OnValueChanged()
        {
            OnOresChanged?.Invoke();
        }
    }
}

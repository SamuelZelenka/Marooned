using System;

public abstract class PointOfInterest
{
    public delegate void PointOfInterestHandler(PointOfInterest pointOfInterest);
    public event PointOfInterestHandler OnInteractedWith;

    public string name;

    public PointOfInterest(string name, PointOfInterestHandler pointOfInterestHandler, Type type)
    {
        this.name = name;
        OnInteractedWith = pointOfInterestHandler;
        MyType = type;
    }

    public void InteractWith() => OnInteractedWith?.Invoke(this);

    public enum Type { Harbor}
    public Type MyType { get; private set; }
}

public class Harbor : PointOfInterest
{
    public MerchantData merchantData; //TEMP (NOT ALL HARBORS HAVE MERCHANTS
    public Harbor(string name, PointOfInterestHandler pointOfInterestHandler) : base(name, pointOfInterestHandler, Type.Harbor)
    {
        //TEMP VALUES
        merchantData.woolValue = UnityEngine.Random.Range(1,11);
        merchantData.tobaccoValue = UnityEngine.Random.Range(2, 12);
        merchantData.coffeeValue = UnityEngine.Random.Range(3, 13);
        merchantData.silkValue = UnityEngine.Random.Range(4, 14);
        merchantData.oreValue = UnityEngine.Random.Range(5, 15);
    }

    public int GetResourceValue(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Wool:
                return merchantData.woolValue;
            case ResourceType.Tobacco:
                return merchantData.tobaccoValue;
            case ResourceType.Coffee:
                return merchantData.coffeeValue;
            case ResourceType.Silk:
                return merchantData.silkValue;
            case ResourceType.Ores:
                return merchantData.oreValue;
        }
        return int.MinValue;
    }

    public struct MerchantData
    {
        public int woolValue;
        public int tobaccoValue;
        public int coffeeValue;
        public int silkValue;
        public int oreValue;
    }
}
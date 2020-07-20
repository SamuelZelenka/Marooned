using System;
using UnityEngine.PlayerLoop;

public abstract class PointOfInterest
{
    public delegate void PointOfInterestHandler(PointOfInterest pointOfInterest);
    public event PointOfInterestHandler OnInteractedWith;

    public string name;

    public PointOfInterest(string name, PointOfInterestHandler pointOfInterestHandler)
    {
        this.name = name;
        OnInteractedWith = pointOfInterestHandler;
    }

    public void InteractWith() => OnInteractedWith?.Invoke(this);
}

public class Harbor : PointOfInterest
{
    public MerchantData merchantData;
    public Harbor(string name, PointOfInterestHandler pointOfInterestHandler) : base(name, pointOfInterestHandler)
    {
        //TEMP VALUES
        merchantData.woolValue = UnityEngine.Random.Range(1,11);
        merchantData.tobaccoValue = UnityEngine.Random.Range(2, 12);
        merchantData.coffeeValue = UnityEngine.Random.Range(3, 13);
        merchantData.silkValue = UnityEngine.Random.Range(4, 14);
        merchantData.oreValue = UnityEngine.Random.Range(5, 15);
    }

    public struct MerchantData
    {
        public int woolValue;
        public int tobaccoValue;
        public int coffeeValue;
        public int silkValue;
        public int oreValue;

        void GetResourceValue(/* INSERT RESOURCE ENUM */)
        {

        }
    }
}
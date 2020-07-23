using System.Collections.Generic;

public class Harbor : PointOfInterest
{
    public delegate void HarborHandler(Harbor harbor);
    public HarborHandler OnHarborChanged;
    public bool hasMerchant;
    public bool hasTavern;



    public Harbor(string name, PointOfInterestHandler pointOfInterestHandler, bool hasMerchant, bool hasTavern) : base(name, pointOfInterestHandler, Type.Harbor)
    {
        //TEMP VALUES
        merchantData.woolValue = UnityEngine.Random.Range(1, 11);
        merchantData.tobaccoValue = UnityEngine.Random.Range(2, 12);
        merchantData.coffeeValue = UnityEngine.Random.Range(3, 13);
        merchantData.silkValue = UnityEngine.Random.Range(4, 14);
        merchantData.oreValue = UnityEngine.Random.Range(5, 15);

        this.hasMerchant = hasMerchant;
        this.hasTavern = hasTavern;
        foodCost = UnityEngine.Random.Range(5, 15);
    }
    #region Merchant
    public MerchantData merchantData; //TEMP (NOT ALL HARBORS HAVE MERCHANTS
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
    #endregion
    #region Tavern

    int foodCost;
    int foodAmount = 10;
    int foodHeal = 2;

    public Character recruitableCharacter { get; set; }

    public bool IsRecruitable(out int cost)
    {
        if (recruitableCharacter == null)
        {
            cost = int.MaxValue;
            return false;
        }
        cost = recruitableCharacter.characterData.BountyLevel.CurrentValue;
        return HexGridController.player.PlayerData.BountyLevel.CurrentValue >= recruitableCharacter.characterData.BountyLevel.CurrentValue;
    }

    public bool CanBuyFood(out int foodCost)
    {
        foodCost = this.foodCost;
        return HexGridController.player.PlayerData.Gold >= foodCost;
    }

    public void RecruitCharacter()
    {
        Character recruitedCharacter = recruitableCharacter;
        HexGridController.SpawnCharacterForPlayerCrew(recruitedCharacter);
        recruitableCharacter = null;
        OnHarborChanged?.Invoke(this);
    }

    public void FeedCharacter(Character character)
    {
        character.characterData.Hunger.CurrentValue += foodAmount;
        character.characterData.Vitality.CurrentValue += foodHeal;
        HexGridController.player.PlayerData.Gold -= foodCost;
    }

    #endregion
}
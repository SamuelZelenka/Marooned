using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterStatType { Strength, Stamina, Constitution, Agility, Toughness, Accuracy}
public enum CharacterResourceType {Vitality, Loyalty, Energy, Hunger, Hygiene, XP }

[Serializable]
public class CharacterData
{
    public int ID;
    //[Range(1, 10)]
    //public int startingLevel = 1;
    public delegate void CharacterDataHandler(CharacterData characterData);
    public delegate void CharacterDataValueHandler(int newValue);

    public event CharacterDataHandler OnCharacterDataInfoRequested;
    public event CharacterDataHandler OnEffectChanged;
    public event CharacterDataHandler OnAnyResourceChanged; 

    [SerializeField] string characterFirstName = "name";
    [SerializeField] string characterLastName = "namesson";

    public string CharacterName
    {
        get
        {
            return $"{characterFirstName} 'The {Bounty.BOUNTYNAMES[BountyLevel.CurrentValue]}' {characterLastName}";  
        }
    }

    public List<TickEffect> activeEffects = new List<TickEffect>();
    public List<TickEffect> removedEffects = new List<TickEffect>();
    public CrewSimulation.ShipJob ShipJob { get; set; } = CrewSimulation.ShipJob.None;

    public const int DEFAULTENERGYREGEN = 20;

    //Resources
    public Resource Vitality { get; private set; } = new Resource("Vitality", 30, 30);
    public Resource Energy { get; private set; } = new Resource("Energy", 100, 100);
    public Resource Hunger { get; private set; } = new Resource("Hunger", 100, 100);
    public Resource Hygiene { get; private set; } = new Resource("Hygiene", 100, 100);
    public Resource Loyalty { get; private set; } = new Resource("Loyalty", 50, 100);
    public Resource XP { get; private set; } = new Resource("XP", 0, 100);

    //Stats
    public Stat Strength { get; private set; } = new Stat("Strength", 1, 20);
    public Stat Stamina { get; private set; } = new Stat("Stamina", 1, 20);
    public Stat Constitution { get; private set; } = new Stat("Constitution", 1, 20);
    public Stat Agility { get; private set; } = new Stat("Agility", 1, 20);
    public Stat Toughness { get; private set; } = new Stat("Toughness", 1, 20);
    public Stat Accuracy { get; private set; } = new Stat("Accuracy", 1, 20);

    public Bounty BountyLevel { get; private set; } = new Bounty("Bounty", 1);



    public CharacterData()
    {
        Vitality.OnResourceChanged += ResourceChanged;
        Energy.OnResourceChanged += ResourceChanged;
        Hunger.OnResourceChanged += ResourceChanged;
        Hygiene.OnResourceChanged += ResourceChanged;
        Loyalty.OnResourceChanged += ResourceChanged;
        XP.OnResourceChanged += ResourceChanged;
    }


    public Resource GetResource(CharacterResourceType resourceType)
    {
        switch (resourceType)
        {
            case CharacterResourceType.Vitality:
                return Vitality;
            case CharacterResourceType.Energy:
                return Energy;
            case CharacterResourceType.Hunger:
                return Hunger;
            case CharacterResourceType.Hygiene:
                return Hygiene;
            case CharacterResourceType.Loyalty:
                return Loyalty;
            case CharacterResourceType.XP:
                return XP;
            default:
                return null;
        }
    }

    public int GetStatValue(CharacterStatType statType)
    {
        Stat stat = GetStat(statType);
        if (stat != null)
        {
            return stat.CurrentValue;
        }
        return int.MinValue;
    }

    public Stat GetStat(CharacterStatType statType)
    {
        switch (statType)
        {
            case CharacterStatType.Strength:
                return Strength;
            case CharacterStatType.Stamina:
                return Stamina;
            case CharacterStatType.Constitution:
                return Constitution;
            case CharacterStatType.Agility:
                return Agility;
            case CharacterStatType.Toughness:
                return Toughness;
            case CharacterStatType.Accuracy:
                return Accuracy;
            default:
                Debug.LogError("Stat not found");
                return null;
        }
    }

    public void AddEffect(TickEffect effect)
    {
        activeEffects.Add(effect);
        OnEffectChanged?.Invoke(this);
        Debug.Log(activeEffects.Count);
    }

    public void RemoveEffects(TickEffect effect)
    {
        if (activeEffects.Contains(effect))
        {
            activeEffects.Remove(effect);
            removedEffects.Add(effect);
            OnEffectChanged?.Invoke(this);
        }
        else
        {
            Debug.LogError($"ActiveEffects does not contain this effect");
        }
    }

    public void SendValuesToRequesters() => OnCharacterDataInfoRequested?.Invoke(this);
    public void ResourceChanged(int newValue) => OnAnyResourceChanged(this);


    [Serializable]
    public class Resource
    {
        public event CharacterDataValueHandler OnResourceChanged;

        const int MINRESOURCEVALUE = 0;

        public string ResourceName { get; private set; }
        public int MaxValue { get; private set; }
        int currentValue;
        public int CurrentValue
        {
            get => currentValue;
            set
            {
                currentValue = Mathf.Clamp(value, MINRESOURCEVALUE, MaxValue);
                OnResourceChanged?.Invoke(currentValue);
            }
        }

        public string Percentage => Utility.FactorToPercentageText((float)currentValue / (float)MaxValue);

        public Resource(string resourceName, int currentValue, int maxValue)
        {
            this.ResourceName = resourceName;
            this.MaxValue = maxValue;
            this.currentValue = currentValue;
        }
        public override string ToString()
        {
            return $"{currentValue} / {MaxValue}";
        }
    }
    [Serializable]
    public class Stat
    {
        public event CharacterDataValueHandler OnStatChanged;

        const int MINSTATVALUE = 1;
        int maxStatValue = 20;

        public string StatName { get; private set; }
        private int currentValue;
        public int CurrentValue 
        {
            get => currentValue;
            private set 
            {
                currentValue = Mathf.Clamp(value, MINSTATVALUE, maxStatValue);
                OnStatChanged?.Invoke(currentValue); 
            }
        }

        public void IncreaseStat(int increase) => CurrentValue += increase;
        public void DecreaseStat(int decrease) => CurrentValue -= decrease;


        public Stat(string statName, int currentValue, int maxValue)
        {
            this.StatName = statName;
            this.currentValue = currentValue;
            this.maxStatValue = maxValue;
        }
        public override string ToString()
        {
            return CurrentValue.ToString();
        }
    }
}


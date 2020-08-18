using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterStatType { Strength, Accuracy, Agility, Fortitude, Intelligence, Charisma, NONE }
public enum CharacterResourceType { Vitality, Loyalty, Energy, Hunger, Hygiene}

[Serializable]
public class CharacterData
{

    public delegate void CharacterDataHandler(CharacterData characterData);
    public delegate void CharacterDataValueHandler(int newValue);

    public event CharacterDataHandler OnCharacterDataInfoRequested;
    public event CharacterDataHandler OnEffectChanged;
    public event CharacterDataHandler OnAnyResourceChanged;

    [Header("Setup")]
    public int ID;
    [SerializeField] string characterFirstName = "name";
    [SerializeField] string characterLastName = "namesson";

    public int recruitBountyDemand = 100;

    [SerializeField] int startVitalityMax = 1;
    [SerializeField] int startStrength = 1;
    [SerializeField] int startAccuracy = 1;
    [SerializeField] int startAgility = 1;
    [SerializeField] int startFortitude = 1;
    [SerializeField] int startIntelligence = 1;
    [SerializeField] int startCharisma = 1;

    public string CharacterName
    {
        get
        {
            //return $"{characterFirstName} 'The {BountyLevel.BOUNTYNAMES[BountyLevel.Level]}' {characterLastName}";  
            return $"{characterFirstName} {characterLastName}";
        }
    }

    public List<TickEffect> activeEffects = new List<TickEffect>();
    public List<TickEffect> removedEffects = new List<TickEffect>();
    public CrewSimulation.ShipJob ShipJob { get; set; } = CrewSimulation.ShipJob.None;

    public const int DEFAULTENERGYREGEN = 20;

    //Resources
    public Resource Vitality { get; private set; }
    public Resource Energy { get; private set; } = new Resource("Energy", 100, 100);
    public Resource Hunger { get; private set; } = new Resource("Hunger", 100, 100);
    public Resource Hygiene { get; private set; } = new Resource("Hygiene", 100, 100);
    public Resource Loyalty { get; private set; } = new Resource("Loyalty", 50, 100);

    //Stats
    public Stat Strength { get; private set; }
    public Stat Accuracy { get; private set; }
    public Stat Agility { get; private set; }
    public Stat Fortitude { get; private set; }
    public Stat Intelligence { get; private set; }
    public Stat Charisma { get; private set; }

    public Level BountyLevel { get; private set; } = new Level(1);



    public void Setup()
    {
        Vitality = new Resource("Vitality", startVitalityMax, startVitalityMax);

        Strength = new Stat("Strength", startStrength, 10);
        Accuracy = new Stat("Accuracy", startAccuracy, 10);
        Agility = new Stat("Agility", startAgility, 10);
        Fortitude = new Stat("Fortitude", startFortitude, 10);
        Intelligence = new Stat("Intelligence", startIntelligence, 10);
        Charisma = new Stat("Charisma", startCharisma, 10);

        Vitality.OnResourceChanged += ResourceChanged;
        Energy.OnResourceChanged += ResourceChanged;
        Hunger.OnResourceChanged += ResourceChanged;
        Hygiene.OnResourceChanged += ResourceChanged;
        Loyalty.OnResourceChanged += ResourceChanged;
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
            case CharacterStatType.Accuracy:
                return Accuracy;
            case CharacterStatType.Agility:
                return Agility;
            case CharacterStatType.Fortitude:
                return Fortitude;
            case CharacterStatType.Intelligence:
                return Intelligence;
            case CharacterStatType.Charisma:
                return Charisma;
            case CharacterStatType.NONE:
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


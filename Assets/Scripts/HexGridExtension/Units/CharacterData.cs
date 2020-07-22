using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterStatType { Strength, Stamina, Constitution, Agility, Toughness, Accuracy, Bounty}
public enum CharacterResourceType {Vitality, Loyalty, Energy, Hunger, Hygiene, XP }

[Serializable]
public class CharacterData
{
    public int ID;

    public delegate void CharacterDataHandler();
    public static event CharacterDataHandler OnResourceChanged;
    public static event CharacterDataHandler OnStatChanged;
    public static event CharacterDataHandler OnEffectChanged;

    [SerializeField] string characterFirstName = "name";
    [SerializeField] string characterLastName = "namesson";

    public string CharacterName
    {
        get
        {
            return $"{characterFirstName} 'The {BOUNTYNAMES[Bounty.CurrentValue]}' {characterLastName}";  
        }
    }
    public static readonly string[] BOUNTYNAMES = new string[] {"Landlubber", "Cannonfodder", "Freebooter", "Seadog", "Skirmisher", "Bootyfinder", "Seasoned", "First mate", "Expert" ,"Legend"};
    public static readonly int[] BOUNTYLEVELS = new int[] { 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };

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

    //Stats
    public Stat Strength { get; private set; } = new Stat("Strength", 1, 20);
    public Stat Stamina { get; private set; } = new Stat("Stamina", 1, 20);
    public Stat Constitution { get; private set; } = new Stat("Constitution", 1, 20);
    public Stat Agility { get; private set; } = new Stat("Agility", 1, 20);
    public Stat Toughness { get; private set; } = new Stat("Toughness", 1, 20);
    public Stat Accuracy { get; private set; } = new Stat("Accuracy", 1, 20);
    public Stat Bounty { get; private set; } = new Stat("Bounty", 1, 10);

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
            case CharacterStatType.Bounty:
                return Bounty;
            default:
                Debug.LogError("Stat not found");
                return null;
        }
    }

    public void AddEffect(TickEffect effect)
    {
        activeEffects.Add(effect);
        OnEffectChanged?.Invoke();
        Debug.Log(activeEffects.Count);
    }

    public void RemoveEffects(TickEffect effect)
    {
        if (activeEffects.Contains(effect))
        {
            activeEffects.Remove(effect);
            removedEffects.Add(effect);
            OnEffectChanged?.Invoke();
        }
        else
        {
            Debug.LogError($"ActiveEffects does not contain this effect");
        }
    }

    public void ObjectInitialized() => OnResourceChanged?.Invoke();

    [Serializable]
    public class Resource
    {
        const int MINRESOURCEVALUE = 0;

        public string resourceName;
        public int maxValue;
        int currentValue;
        public int CurrentValue
        {
            get => currentValue;
            set
            {
                currentValue = Mathf.Clamp(value, MINRESOURCEVALUE, maxValue);
                OnResourceChanged?.Invoke();
            }
        }

        public string Percentage => Utility.FactorToPercentageText((float)currentValue / (float)maxValue);

        public Resource(string resourceName, int currentValue, int maxValue)
        {
            this.resourceName = resourceName;
            this.maxValue = maxValue;
            this.currentValue = currentValue;
        }
        public override string ToString()
        {
            return $"{currentValue} / {maxValue}";
        }
    }
    [Serializable]
    public class Stat
    {
        const int MINSTATVALUE = 1;
        int maxStatValue = 20;

        public string statName;
        private int currentValue;
        public int CurrentValue 
        {
            get => currentValue;
            private set 
            {
                currentValue = Mathf.Clamp(value, MINSTATVALUE, maxStatValue);
                OnStatChanged?.Invoke(); 
            }
        }

        public void IncreaseStat(int increase) => CurrentValue += increase;
        public void DecreaseStat(int decrease) => CurrentValue -= decrease;


        public Stat(string statName, int currentValue, int maxValue)
        {
            this.statName = statName;
            this.currentValue = currentValue;
            this.maxStatValue = maxValue;
        }
        public override string ToString()
        {
            return CurrentValue.ToString();
        }
    }

}


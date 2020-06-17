using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterData
{
    public string characterName = "name";

    public List<Effect> activeEffects = new List<Effect>();
    public List<Effect> removedEffects = new List<Effect>();
    public CrewSimulation.ShipJob ShipJob { get; set; } = CrewSimulation.ShipJob.None;

    #region Visuals
    public Sprite portrait = null;
    public Sprite inGameSprite = null;
    #endregion

    //Resources
    public Resource Vitality { get; private set; } = new Resource("Vitality", 30, 30);
    public Resource Energy { get; private set; } = new Resource("Energy", 100, 100);
    public Resource Hunger { get; private set; } = new Resource("Hunger", 100, 100);
    public Resource Hygiene { get; private set; } = new Resource("Hygiene", 100, 100);
    public Resource Loyalty { get; private set; } = new Resource("Loyalty", 50, 100);
    public Resource XP { get; private set; } = new Resource("XP", 0, 10);

    //Stats
    public Stat Strength { get; private set; } = new Stat("Strength", 1);
    public Stat Stamina { get; private set; } = new Stat("Stamina", 1);
    public Stat Constitution { get; private set; } = new Stat("Constitution", 1);
    public Stat Agility { get; private set; } = new Stat("Agility", 1);
    public Stat Toughness { get; private set; } = new Stat("Toughness", 1);
    public Stat Accuracy { get; private set; } = new Stat("Accuracy", 1);
    public Stat Bounty { get; private set; } = new Stat("Bounty", 1);



    //[SerializeField] int vitality = 30;
    //[SerializeField] int maxVitality = 30;
    //[SerializeField] int energy = 1;
    //[SerializeField] int maxEnergy = 100;
    //[SerializeField] int hunger = 1;
    //[SerializeField] int maxHunger = 1;
    //[SerializeField] int hygiene = 1;
    //[SerializeField] int maxHygiene = 100;
    //[SerializeField] int loyalty = 1;
    //[SerializeField] int maxLoyalty = 100;


    ////Stats
    //int strength = 0;
    //int stamina = 0;
    //int constitution = 0;
    //int agility = 0;
    //int toughness = 0;
    //int accuracy = 0;
    //int bounty = 0;
    //#region Stats
    //public int Strength
    //{
    //    get { return strength; }
    //    set
    //    {
    //        strength = value;
    //    }
    //}
    //public int Stamina
    //{
    //    get { return stamina; }
    //    set
    //    {
    //        stamina = value;
    //    }
    //}
    //public int Constitutuion
    //{
    //    get { return constitution; }
    //    set
    //    {
    //        constitution = value;
    //    }
    //}
    //public int Agility
    //{
    //    get { return agility; }
    //    set
    //    {
    //        agility = value;
    //    }
    //}
    //public int Toughness
    //{
    //    get { return toughness; }
    //    set
    //    {
    //        toughness = value;
    //    }
    //}
    //public int Accuracy
    //{
    //    get { return accuracy; }
    //    set
    //    {
    //        accuracy = value;
    //    }
    //}
    //public int Bounty
    //{
    //    get { return bounty; }
    //    set
    //    {
    //        bounty = value;
    //    }
    //}
    //#endregion
    //#region Resources
    //public int MaxVitality
    //{
    //    get { return maxVitality; }
    //    set
    //    {
    //        maxVitality = value;
    //    }
    //}
    //public int Vitality
    //{
    //    get { return vitality; }
    //    set
    //    {
    //        vitality = Mathf.Clamp(value, MINRESOURCEVALUE, maxVitality);
    //        if (vitality <= 0)
    //        {
    //            Debug.Log("Is dead");
    //        }
    //    }
    //}
    //public int MaxEnergy
    //{
    //    get { return maxEnergy; }
    //    set
    //    {
    //        maxEnergy = value;
    //    }
    //}
    //public int Energy
    //{
    //    get { return energy; }
    //    set
    //    {
    //        energy = Mathf.Clamp(value, MINRESOURCEVALUE, maxEnergy);
    //    }
    //}

    //public int MaxHunger
    //{
    //    get { return maxHunger; }
    //    set
    //    {
    //        maxHunger = value;
    //    }
    //}
    //public int Hunger
    //{
    //    get { return hunger; }
    //    set
    //    {
    //        hunger = Mathf.RoundToInt(Mathf.Min(value, 1));
    //    }
    //}
    //public int MaxHygiene
    //{
    //    get { return maxHunger; }
    //    set
    //    {
    //        maxHunger = value;
    //    }
    //}
    //public float Hygiene
    //{
    //    get { return hygiene; }
    //    set
    //    {
    //        hygiene = Mathf.RoundToInt(Mathf.Min(value, 1));
    //    }
    //}
    //public int MaxLoyalty
    //{
    //    get { return maxHunger; }
    //    set
    //    {
    //        maxHunger = value;
    //    }
    //}
    //public float Loyalty
    //{
    //    get { return loyalty; }
    //    set
    //    {
    //        loyalty = Mathf.RoundToInt(Mathf.Min(value, 1));
    //    }
    //}
    //#endregion

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
            }
        }

        public string Percentage => Utility.FactorToPercentageText((float)currentValue / (float)maxValue);

        public Resource(string resourceName, int currentValue, int maxValue)
        {
            this.resourceName = resourceName;
            this.CurrentValue = currentValue;
            this.maxValue = maxValue;
        }
    }

    public class Stat
    {
        const int MINSTATVALUE = 1;
        const int MAXSTATVALUE = 20;

        public string statName;
        public int CurrentValue { get; private set; }

        public void IncreaseStat(int increase) => CurrentValue += increase;

        public Stat(string statName, int currentValue)
        {
            this.statName = statName;
            this.CurrentValue = currentValue;
        }
    }
}


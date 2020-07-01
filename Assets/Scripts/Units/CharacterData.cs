﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterStatType { Strength, Stamina, Constituation, Agility, Toughness, Accuracy}

[Serializable]
public class CharacterData
{
    public string characterName = "name";

    public List<TickEffect> activeEffects = new List<TickEffect>();
    public List<TickEffect> removedEffects = new List<TickEffect>();
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

    public int GetStatValue(CharacterStatType statType)
    {
        switch (statType)
        {
            case CharacterStatType.Strength:
                return Strength.CurrentValue;
            case CharacterStatType.Stamina:
                return Stamina.CurrentValue;
            case CharacterStatType.Constituation:
                return Constitution.CurrentValue;
            case CharacterStatType.Agility:
                return Agility.CurrentValue;
            case CharacterStatType.Toughness:
                return Toughness.CurrentValue;
            case CharacterStatType.Accuracy:
                return Accuracy.CurrentValue;
            default:
                Debug.LogError("Stat not found");
                return int.MinValue;
        }
    }

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
            }
        }

        public string Percentage => Utility.FactorToPercentageText((float)currentValue / (float)maxValue);

        public Resource(string resourceName, int currentValue, int maxValue)
        {
            this.resourceName = resourceName;
            this.maxValue = maxValue;
            this.CurrentValue = currentValue;
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
        const int MAXSTATVALUE = 20;

        public string statName;
        public int CurrentValue { get; private set; }

        public void IncreaseStat(int increase) => CurrentValue += increase;

        public Stat(string statName, int currentValue)
        {
            this.statName = statName;
            this.CurrentValue = currentValue;
        }
        public override string ToString()
        {
            return CurrentValue.ToString();
        }
    }
}


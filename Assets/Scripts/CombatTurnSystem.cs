using System.Collections.Generic;
using UnityEngine;
using System;

public class CombatTurnSystem : MonoBehaviour
{
    public delegate void CharacterHandler(Character character);
    public static CharacterHandler OnTurnEnding;
    public static CharacterHandler OnTurnBegining;

    public static Queue<Character> TurnOrder
    {
        private set;
        get;
    }

    /// <summary>
    /// Initializing the settings and turnorder for the combat
    /// </summary>
    /// <param name="allCharacters"></param>
    public void SetupNewCombat(List<Character> allCharacters)
    {
        TurnOrder = new Queue<Character>();
        CharacterInitiative[] characterInitiatives = new CharacterInitiative[allCharacters.Count];
        for (int i = 0; i < allCharacters.Count; i++)
        {
            characterInitiatives[i] = new CharacterInitiative(allCharacters[i]);
            characterInitiatives[i].initiative = allCharacters[i].characterData.Agility.CurrentValue + AddRandomizer();
        }

        Array.Sort(characterInitiatives);

        for (int i = 0; i < characterInitiatives.Length; i++)
        {
            TurnOrder.Enqueue(characterInitiatives[i].character);
            Debug.Log(characterInitiatives[i].initiative);
        }
    }

    int numberOfRandomizers = 4;
    private int AddRandomizer()
    {
        int value = 0;
        for (int i = 0; i < numberOfRandomizers; i++)
        {
            if (UnityEngine.Random.Range(0, 2) == 1)
            {
                value += 1;
            }
            else
            {
                value -= 1;
            }
        }
        return value;
    }

    /// <summary>
    ///Starts the first turn in the combat
    /// </summary>
    public void StartCombat()
    {
        StartNextTurn();
    }

    //Called from UI to end player character turn or when an ability is used
    public void EndActiveCharacterTurn()
    {
        OnTurnEnding?.Invoke(HexGridController.ActiveCharacter);
        HexGridController.ActiveCharacter.TurnEnded();
        TurnOrder.Enqueue(HexGridController.ActiveCharacter);
        StartNextTurn();
    }

    //Starting the turn for the character in the first position in the queue
    private void StartNextTurn()
    {
        HexGridController.ActiveCharacter = TurnOrder.Dequeue();
        Debug.Log("Starting turn for " + HexGridController.ActiveCharacter.characterData.CharacterName);

        OnTurnBegining?.Invoke(HexGridController.ActiveCharacter);

        if (!HexGridController.ActiveCharacter.playerControlled)
        {
            StartCoroutine(HexGridController.ActiveCharacter.PerformAutomaticTurn());
        }
    }

    //Private class
    class CharacterInitiative : IComparable
    {
        public Character character;
        public int initiative;

        public CharacterInitiative(Character character)
        {
            this.character = character;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            CharacterInitiative otherInitiative = obj as CharacterInitiative;

            if (otherInitiative != null)
            {
                return otherInitiative.initiative.CompareTo(this.initiative);
            }
            else
            {
                throw new ArgumentException("Object is not a Character Initiative");
            }
        }
    }
}

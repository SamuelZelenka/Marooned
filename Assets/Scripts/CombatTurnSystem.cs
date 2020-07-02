using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CombatTurnSystem : MonoBehaviour
{
    public delegate void CharacterHandler(Character character);
    public static CharacterHandler OnTurnEnding;
    public static CharacterHandler OnTurnBegining;

    public CombatUIController uIController;

    Queue<Character> turnOrder;

    private void OnEnable()
    {
        //Subscribe
        OnTurnEnding += ChangeActiveCharacter;
        CombatSystem.OnAbilityUsed += EndActiveCharacterTurn;
    }

    private void OnDisable()
    {
        //Un-subscribe
        OnTurnEnding -= ChangeActiveCharacter;
        CombatSystem.OnAbilityUsed -= EndActiveCharacterTurn;
    }

    /// <summary>
    /// Initializing the settings and turnorder for the combat
    /// </summary>
    /// <param name="allCharacters"></param>
    public void SetupNewCombat(List<Character> allCharacters)
    {
        turnOrder = new Queue<Character>();
        CharacterInitiative[] characterInitiatives = new CharacterInitiative[allCharacters.Count];
        for (int i = 0; i < allCharacters.Count; i++)
        {
            characterInitiatives[i] = new CharacterInitiative(allCharacters[i]);
            characterInitiatives[i].initiative = allCharacters[i].characterData.Agility.CurrentValue + AddRandomizer();
        }

        Array.Sort(characterInitiatives);

        for (int i = 0; i < characterInitiatives.Length; i++)
        {
            turnOrder.Enqueue(characterInitiatives[i].character);
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

    //Called from UI to end player character turn
    public void EndActiveCharacterTurn()
    {
        OnTurnEnding?.Invoke(HexGridController.ActiveCharacter);
    }

    //Subscribed method to change turn when one character is declared done with its turn
    private void ChangeActiveCharacter(Character lastCharacter)
    {
        turnOrder.Enqueue(lastCharacter);
        StartNextTurn();
    }

    //Starting the turn for the character in the first position in the queue
    private void StartNextTurn()
    {
        //De-select last active character
        if (HexGridController.ActiveCharacter)
        {
            HexGridController.ActiveCharacter.ShowUnitActive(false);
        }
        HexGridController.ActiveCharacter = turnOrder.Dequeue();
        if (!HexGridController.SelectedCharacter)
        {
            HexGridController.SelectedCell = HexGridController.ActiveCharacter.Location;
        }
        Debug.Log("Starting turn for " + HexGridController.ActiveCharacter.characterData.characterName);

        OnTurnBegining?.Invoke(HexGridController.ActiveCharacter);

        HexGridController.ActiveCharacter.StartNewTurn();
        if (!HexGridController.ActiveCharacter.playerControlled)
        {
            StartCoroutine(HexGridController.ActiveCharacter.PerformAutomaticTurn());
        }
        uIController.UpdateTimeline(turnOrder.ToList());
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

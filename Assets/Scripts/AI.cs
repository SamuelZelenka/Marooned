using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AI
{
    List<Character> myCharacters = new List<Character>();
    List<Character> enemies = new List<Character>();
    float difficulty = 0.9f; //Used to randomize chosen actions

    private void CharacterDied(Character character)
    {
        if (myCharacters.Contains(character))
        {
            myCharacters.Remove(character);
        }
        else if (enemies.Contains(character))
        {
            enemies.Remove(character);
        }
    }

    public AI(List<Character> myCharacters, List<Character> enemies)
    {
        this.myCharacters.AddRange(myCharacters);
        this.enemies.AddRange(enemies);
        //Character.Oncharacterdied += CharacterDied
    }

    public IEnumerator CalculateAvailableActions(Character activeCharacter)
    {
        List<ActionGroup> possibleActions = new List<ActionGroup>();

        //Using ability from current location
        foreach (var ability in activeCharacter.Abilities)
        {
            if (activeCharacter.characterData.Energy.CurrentValue < ability.cost) //Not enough energ
            {
                continue;
            }
            foreach (var targetCell in ability.targeting.GetValidTargets(activeCharacter.Location))
            {
                possibleActions.Add(new ActionGroup(activeCharacter.Location, targetCell, ActionGroup.ActionChoice.AbilityOnly, ability));
            }
        }

        //Moving
        foreach (var moveToCell in activeCharacter.ReachableCells)
        {
            //Only move
            possibleActions.Add(new ActionGroup(moveToCell, null, ActionGroup.ActionChoice.WalkOnly, null));


            //Using abilities after move
            foreach (var ability in activeCharacter.Abilities)
            {
                if (activeCharacter.characterData.Energy.CurrentValue < ability.cost) //Not enough energy
                {
                    continue;
                }
                foreach (var targetCell in ability.targeting.GetValidTargets(moveToCell))
                {
                    possibleActions.Add(new ActionGroup(moveToCell, targetCell, ActionGroup.ActionChoice.WalkAndAbility, ability));
                }
            }
        }

        Debug.Log("Number of actions is " + possibleActions.Count);
        //Score all actiongroups
        foreach (var item in possibleActions)
        {
            Score(item, activeCharacter);
            yield return null; //Wait one frame
        }

        //Sort
        possibleActions.Sort();

        //Pick one actiongroup
        int indexToChoose = Mathf.RoundToInt(UnityEngine.Random.Range(1 - difficulty, 0) * possibleActions.Count);
        indexToChoose = Mathf.Min(indexToChoose, possibleActions.Count - 1);
        ActionGroup chosenAction = possibleActions[indexToChoose];
        activeCharacter.SetNextAction(chosenAction);
    }

    void Score(ActionGroup actionGroup, Character activeCharacter)
    {
        int score = 0;
        if (actionGroup.abilityToUse != null)
        {
            //Check taunt
            bool isTargettingTauntingCharacter = false;
            Character potentialTaunter = actionGroup.cellAbilityTarget.Unit as Character;
            if (potentialTaunter != null && activeCharacter.tauntedBy.Contains(potentialTaunter))
            {
                isTargettingTauntingCharacter = true;
            }

            //Check number of targets
            int targets = 0;
            List<HexCell> affectedCells = actionGroup.abilityToUse.targeting.GetAffectedCells(actionGroup.cellToEndTurnOn, actionGroup.cellAbilityTarget);
            foreach (HexCell cell in affectedCells)
            {
                Character potentialEnemy = cell.Unit as Character;
                if (potentialEnemy != null && enemies.Contains(potentialEnemy))
                {
                    targets++;
                }
            }

            //Score
            score = actionGroup.abilityToUse.cost;
            score *= targets;
            score = isTargettingTauntingCharacter ? score * 2 : score;
        }
        actionGroup.score = score;
    }
}

public class ActionGroup : IComparable
{
    public HexCell cellToEndTurnOn;
    public HexCell cellAbilityTarget;
    public enum ActionChoice { WalkAndAbility, WalkOnly, AbilityOnly }
    public ActionChoice typeOfAction;
    public Ability abilityToUse;

    public int score;

    public ActionGroup(HexCell cellToEndTurnOn, HexCell cellAbilityTarget, ActionChoice typeOfAction, Ability abilityToUse)
    {
        this.cellToEndTurnOn = cellToEndTurnOn;
        this.cellAbilityTarget = cellAbilityTarget;
        this.typeOfAction = typeOfAction;
        this.abilityToUse = abilityToUse;
    }

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;

        ActionGroup otherAction = obj as ActionGroup;

        if (otherAction != null)
        {
            return otherAction.score.CompareTo(this.score);
        }
        else
        {
            throw new ArgumentException("Object is not a Character Initiative");
        }
    }
}

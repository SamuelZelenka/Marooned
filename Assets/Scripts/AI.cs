using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI
{
    List<Character> myCharacters = new List<Character>();
    List<Character> enemies = new List<Character>();

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
            Score(item);
            yield return null; //Wait one frame
        }

        //Sort
        foreach (var item in possibleActions)
        {
            //Sort(item);
        }

        //Pick one actiongroup

        yield return null;
    }

    int Score(ActionGroup actionGroup)
    {
        return 0;
    }
}

public class ActionGroup
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
}

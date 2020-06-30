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

    IEnumerator Score(Character activeCharacter)
    {
        yield return null;
    }
}

public struct ActionGroup
{
    public HexCell target;
    public enum ActionChoice { Move, UseAbility}
    public ActionChoice actionChoice;
    public int abilityIndex;

    public int score;
}

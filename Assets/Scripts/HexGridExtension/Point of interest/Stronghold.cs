using UnityEngine;
using System.Collections.Generic;

public class Stronghold : PointOfInterest
{
    public List<Challenge> challenges;

    public Stronghold(string name, HexCell hexCell, DifficultySettings difficultySettings, PointOfInterestHandler pointOfInterestHandler) : base(name, hexCell, pointOfInterestHandler, Type.Stronghold)
    {
        challenges = new List<Challenge>();

        int numberOfChallenges = Random.Range(difficultySettings.strongholdMinChallenges, difficultySettings.strongholdMaxChallenges + 1);
        int combatIndex = Random.Range(0, numberOfChallenges);
        for (int i = 0; i < numberOfChallenges; i++)
        {
            if (i == combatIndex)
            {
                challenges.Add(new Challenge(Challenge.Type.Combat, CharacterStatType.NONE));
            }
            else
            {
                challenges.Add(new Challenge(Challenge.Type.SkillcheckChallenge, (CharacterStatType)Random.Range(0, (int)CharacterStatType.Charisma) + 1));
            }
        }
        Debug.Log("Stronghold made");
    }
}

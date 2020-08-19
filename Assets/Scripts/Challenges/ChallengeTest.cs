using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeTest : MonoBehaviour
{
    public ChallengeManager challengeManager;
    public int challengeLevel;
    public int characterSkillLevel;

    public int numberOfTests = 100;

    private void Start()
    {
        for (int i = 0; i < numberOfTests; i++)
        {
            TestChallenge();
        }
    }
    public void TestChallenge()
    {
        challengeManager.StartChallenge(new Challenge(Challenge.Type.SkillcheckChallenge, CharacterStatType.Accuracy, challengeLevel));
        challengeManager.SetCharacter(characterSkillLevel);
    }

}

using UnityEngine;

[SerializeField]
public class Challenge
{
    public enum Type { SkillcheckChallenge, Combat }
    public Type challengeType;
    public CharacterStatType skillcheckType;
    public Challenge(Type challengeType, CharacterStatType skillcheckType)
    {
        this.challengeType = challengeType;
        this.skillcheckType = skillcheckType;
    }
}

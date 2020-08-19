using UnityEngine;

[SerializeField]
public class Challenge
{
    public enum Type { SkillcheckChallenge, Combat }
    public Type challengeType;
    public CharacterStatType skillcheckType;
    public int challengeLevel;

    public string description = null;

    public string criticalSuccessEffectText = null;
    public string successEffectText = null;
    public string failEffectText = null;
    public string criticalFailEffectText = null;


    public Challenge(Type challengeType, CharacterStatType skillcheckType, int challengeLevel)
    {
        this.challengeType = challengeType;
        this.skillcheckType = skillcheckType;
        this.challengeLevel = challengeLevel;
    }
}

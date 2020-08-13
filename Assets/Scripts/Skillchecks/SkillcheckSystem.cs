using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public class SkillcheckSystem : MonoBehaviour
//{
//    public const int NUMBEROFCOINS = 4;
//    //public enum SkillcheckRequirement { None, Strength, Accuracy, Agility, Stamina, Constitution, Toughness }
//    public enum CombatOutcome { Miss, Grace, NormalHit, Critical }

//    public delegate void CombatOutcomeHandler(List<CombatOutcome> hostileOutcomes, List<CombatOutcome> friendlyOutcomes);
//    public CombatOutcomeHandler OnCombatOutcomesDecided;

//    public delegate void SkillcheckSystemHandler();
//    public static event SkillcheckSystemHandler OnCrit;

//    [Header("References")]
//    [SerializeField] GameObject parent = null;
//    [SerializeField] SkillcheckContester abilityUserSkillcheckSystem = null;
//    [SerializeField] SkillcheckContester[] hostileSkillcheckSystems = null;
//    [SerializeField] SkillcheckContester[] friendlySkillcheckSystems = null;

//    [SerializeField] float timeBeforeCoinFlips = 0.2f, timeBeforePanelClose = 1f;
//    [SerializeField] bool closeAfterCompleted = true;

//    public void StartContestedSkillcheck
//        (
//        Character abilityUser, List<Character> hostileTargets, List<Character> friendlyTargets,
//        SkillcheckRequirement abilityUserSkillcheck, SkillcheckRequirement hostileDodgeSkillcheck, SkillcheckRequirement friendlyDodgeSkillcheck
//        )
//    {
//        parent.SetActive(true);

//        abilityUserSkillcheckSystem.SetupSkillcheck(GetStatValue(abilityUser, abilityUserSkillcheck), abilityUser.portrait);

//        //Hide all defenders
//        foreach (var item in hostileSkillcheckSystems)
//        {
//            item.gameObject.SetActive(false);
//        }
//        foreach (var item in friendlySkillcheckSystems)
//        {
//            item.gameObject.SetActive(false);
//        }

//        for (int i = 0; i < hostileTargets.Count; i++)
//        {
//            hostileSkillcheckSystems[i].gameObject.SetActive(true);
//            hostileSkillcheckSystems[i].SetupSkillcheck(GetStatValue(hostileTargets[i], hostileDodgeSkillcheck), hostileTargets[i].portrait);
//        }

//        for (int i = 0; i < friendlyTargets.Count; i++)
//        {
//            friendlySkillcheckSystems[i].gameObject.SetActive(true);
//            friendlySkillcheckSystems[i].SetupSkillcheck(GetStatValue(friendlyTargets[i], friendlyDodgeSkillcheck), friendlyTargets[i].portrait);
//        }

//        StartCoroutine(PerformContestedSkillcheck(hostileTargets.Count, friendlyTargets.Count));
//    }

//    IEnumerator PerformContestedSkillcheck(int numberOfHostileTargets, int numberOfFriendlyTargets)
//    {
//        yield return new WaitForSeconds(timeBeforeCoinFlips);

//        int userSuccesses = 0;
//        List<bool> userFlips = SkillcheckContester.GetCoinflipResults();

//        for (int i = 0; i < userFlips.Count; i++)
//        {
//            if (userFlips[i])
//            {
//                userSuccesses++;
//            }
//        }
//        yield return abilityUserSkillcheckSystem.FlipCoins(userFlips);

//        CombatOutcome userOutcome = DecideUserOutcome(userSuccesses);
//        List<CombatOutcome> hostileOutcomes = new List<CombatOutcome>(numberOfHostileTargets);
//        List<CombatOutcome> friendlyOutcomes = new List<CombatOutcome>(numberOfFriendlyTargets);

//        //If crit or miss, no need for other flips, just show crit or miss
//        if (userOutcome == CombatOutcome.Critical || userOutcome == CombatOutcome.Miss) //Decided outcome
//        {
//            abilityUserSkillcheckSystem.SetOutcomeText(OutcomeString(userOutcome));
//            for (int i = 0; i < numberOfHostileTargets; i++)
//            {
//                hostileSkillcheckSystems[i].SetOutcomeText(OutcomeString(userOutcome));
//                hostileOutcomes.Add(userOutcome);
//            }
//            for (int i = 0; i < numberOfFriendlyTargets; i++)
//            {
//                friendlySkillcheckSystems[i].SetOutcomeText(OutcomeString(userOutcome));
//                friendlyOutcomes.Add(userOutcome);
//            }
//        }
//        else //If possibly a regular or grace hit
//        {
//            for (int i = 0; i < numberOfHostileTargets; i++)
//            {
//                StartCoroutine(hostileSkillcheckSystems[i].FlipCoins(SkillcheckContester.GetCoinflipResults()));
//            }
//            for (int i = 0; i < numberOfFriendlyTargets; i++)
//            {
//                StartCoroutine(friendlySkillcheckSystems[i].FlipCoins(SkillcheckContester.GetCoinflipResults()));
//            }

//            yield return new WaitForSeconds(SkillcheckCoin.FLIPTIME * NUMBEROFCOINS);

//            bool completed = false;
//            while (!completed)
//            {
//                yield return null;
//                completed = true;
//                for (int i = 0; i < numberOfHostileTargets; i++)
//                {
//                    if (!hostileSkillcheckSystems[i].done)
//                    {
//                        completed = false;
//                    }
//                }
//                for (int i = 0; i < numberOfFriendlyTargets; i++)
//                {
//                    if (!friendlySkillcheckSystems[i].done)
//                    {
//                        completed = false;
//                    }
//                }
//            }

//            for (int i = 0; i < numberOfHostileTargets; i++)
//            {
//                if (CheckResult(abilityUserSkillcheckSystem.EndValue, hostileSkillcheckSystems[i].EndValue))
//                {
//                    hostileSkillcheckSystems[i].SetOutcomeText(OutcomeString(userOutcome));
//                    hostileOutcomes.Add(userOutcome);
//                }
//                else
//                {
//                    hostileSkillcheckSystems[i].SetOutcomeText(OutcomeString(CombatOutcome.Miss));
//                    hostileOutcomes.Add(CombatOutcome.Miss);
//                }
//            }
//            for (int i = 0; i < numberOfFriendlyTargets; i++)
//            {
//                if (CheckResult(abilityUserSkillcheckSystem.EndValue, friendlySkillcheckSystems[i].EndValue))
//                {
//                    friendlySkillcheckSystems[i].SetOutcomeText(OutcomeString(userOutcome));
//                    friendlyOutcomes.Add(userOutcome);
//                }
//                else
//                {
//                    friendlySkillcheckSystems[i].SetOutcomeText(OutcomeString(CombatOutcome.Miss));
//                    friendlyOutcomes.Add(CombatOutcome.Miss);
//                }
//            }
//        }

//        yield return new WaitForSeconds(timeBeforePanelClose);
//        EndContestedSkillcheck(hostileOutcomes, friendlyOutcomes);
//    }



//    private CombatOutcome DecideUserOutcome(int userSuccesses)
//    {
//        CombatOutcome attackerOutcome = CombatOutcome.NormalHit;
//        if (userSuccesses == NUMBEROFCOINS) //All successes = crit always
//        {
//            OnCrit?.Invoke();
//            attackerOutcome = CombatOutcome.Critical;
//        }
//        else if (userSuccesses == 0) //All misses = miss always
//        {
//            attackerOutcome = CombatOutcome.Miss;
//        }
//        else if (userSuccesses == 1) //Grace
//        {
//            attackerOutcome = CombatOutcome.Grace;
//        }
//        return attackerOutcome;
//    }

//    private bool CheckResult(int attackerValue, int defenderValue) => attackerValue >= defenderValue;

//    private string OutcomeString(CombatOutcome combatOutcome)
//    {
//        switch (combatOutcome)
//        {
//            case CombatOutcome.Miss:
//                return "Miss";
//            case CombatOutcome.Grace:
//                return "Grace";
//            case CombatOutcome.NormalHit:
//                return "Hit";
//            case CombatOutcome.Critical:
//                return "Critical Hit";
//            default:
//                return "";
//        }
//    }

//    private void EndContestedSkillcheck(List<CombatOutcome> hostileOutcomes, List<CombatOutcome> friendlyOutcomes)
//    {
//        if (closeAfterCompleted)
//        {
//            foreach (var item in hostileSkillcheckSystems)
//            {
//                item.gameObject.SetActive(false);
//            }

//            parent.SetActive(false);
//        }
//        OnCombatOutcomesDecided?.Invoke(hostileOutcomes, friendlyOutcomes);
//    }

//    private int GetStatValue(Character character, SkillcheckRequirement skillcheckRequirement)
//    {
//        switch (skillcheckRequirement)
//        {
//            case SkillcheckRequirement.Strength:
//                return character.characterData.GetStatValue(CharacterStatType.Strength);
//            case SkillcheckRequirement.Accuracy:
//                return character.characterData.GetStatValue(CharacterStatType.Accuracy);
//            case SkillcheckRequirement.Agility:
//                return character.characterData.GetStatValue(CharacterStatType.Agility);
//            case SkillcheckRequirement.Stamina:
//                return character.characterData.GetStatValue(CharacterStatType.Stamina);
//            case SkillcheckRequirement.Constitution:
//                return character.characterData.GetStatValue(CharacterStatType.Constitution);
//            case SkillcheckRequirement.Toughness:
//                return character.characterData.GetStatValue(CharacterStatType.Toughness);
//            case SkillcheckRequirement.None:
//            default:
//                Debug.LogError("Faulty skillcheck requirement");
//                return int.MinValue;
//        }
//    }
//}

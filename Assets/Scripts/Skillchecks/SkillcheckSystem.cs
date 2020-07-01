using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillcheckSystem : MonoBehaviour
{
    public const int NUMBEROFCOINS = 4;

    public enum CombatOutcome { Miss, Grace, NormalHit, Critical }

    public delegate void CombatOutcomeHandler(List<CombatOutcome> combatOutcomes);
    public CombatOutcomeHandler OnCombatOutcomesDecided;

    [Header("References")]
    [SerializeField] GameObject parent = null;
    [SerializeField] SkillcheckContester skillcheckAttacker = null;
    [SerializeField] SkillcheckContester[] skillcheckTarget = null;

    [SerializeField] float timeBeforeCoinFlips = 0.2f, timeBeforePanelClose = 1f;
    [SerializeField] bool closeAfterCompleted = true;

    public void StartContestedSkillcheck(Character attacker, List<Character> targets, CharacterStatType attackerSkillcheck, CharacterStatType targetSkillcheck)
    {
        parent.SetActive(true);

        skillcheckAttacker.SetupSkillcheck(attacker.characterData.GetStatValue(attackerSkillcheck), attacker.characterData.portrait);

        //Hide all defenders
        foreach (var item in skillcheckTarget)
        {
            item.gameObject.SetActive(false);
        }

        for (int i = 0; i < targets.Count; i++)
        {
            skillcheckTarget[i].gameObject.SetActive(true);
            skillcheckTarget[i].SetupSkillcheck(targets[i].characterData.GetStatValue(targetSkillcheck), targets[i].characterData.portrait);
        }

        StartCoroutine(PerformContestedSkillcheck(targets.Count));
    }

    IEnumerator PerformContestedSkillcheck(int numberOfDefenders)
    {
        yield return new WaitForSeconds(timeBeforeCoinFlips);


        int attackerSuccesses = 0;
        List<bool> attackerFlips = SkillcheckContester.GetCoinflipResults();

        for (int i = 0; i < attackerFlips.Count; i++)
        {
            if (attackerFlips[i])
            {
                attackerSuccesses++;
            }
        }
        yield return skillcheckAttacker.FlipCoins(attackerFlips);

        CombatOutcome attackerOutcome = DecideAttackerOutcome(attackerSuccesses);
        List<CombatOutcome> outcomes = new List<CombatOutcome>(numberOfDefenders);

        //If crit or miss, no need for other flips, just show crit or miss
        if (attackerOutcome == CombatOutcome.Critical || attackerOutcome == CombatOutcome.Miss) //Decided outcome
        {
            skillcheckAttacker.SetOutcomeText(OutcomeString(attackerOutcome));
            for (int i = 0; i < numberOfDefenders; i++)
            {
                skillcheckTarget[i].SetOutcomeText(OutcomeString(attackerOutcome));
                outcomes.Add(attackerOutcome);
            }
        }
        else //If possibly a regular or grace hit
        {
            for (int i = 0; i < numberOfDefenders; i++)
            {
                StartCoroutine(skillcheckTarget[i].FlipCoins(SkillcheckContester.GetCoinflipResults()));
            }

            yield return new WaitForSeconds(SkillcheckCoin.FLIPTIME * NUMBEROFCOINS);

            bool completed = false;
            while(!completed)
            {
                yield return null;
                completed = true;
                for (int i = 0; i < numberOfDefenders; i++)
                {
                    if (!skillcheckTarget[i].done)
                    {
                        completed = false;
                    }
                }
            }

            for (int i = 0; i < numberOfDefenders; i++)
            {
                if (CheckResult(skillcheckAttacker.EndValue, skillcheckTarget[i].EndValue))
                {
                    skillcheckTarget[i].SetOutcomeText(OutcomeString(attackerOutcome));
                    outcomes.Add(attackerOutcome);
                }
                else
                {
                    skillcheckTarget[i].SetOutcomeText(OutcomeString(CombatOutcome.Miss));
                    outcomes.Add(CombatOutcome.Miss);
                }
            }
        }

        yield return new WaitForSeconds(timeBeforePanelClose);
        EndContestedSkillcheck(outcomes);
    }



    private CombatOutcome DecideAttackerOutcome(int attackerSuccesses)
    {
        CombatOutcome attackerOutcome = CombatOutcome.NormalHit;
        if (attackerSuccesses == NUMBEROFCOINS) //All successes = crit always
        {
            attackerOutcome = CombatOutcome.Critical;
        }
        else if (attackerSuccesses == 0) //All misses = miss always
        {
            attackerOutcome = CombatOutcome.Miss;
        }
        else if (attackerSuccesses == 1) //Grace
        {
            attackerOutcome = CombatOutcome.Grace;
        }
        return attackerOutcome;
    }

    private bool CheckResult(int attackerValue, int defenderValue) => attackerValue >= defenderValue;

    private string OutcomeString(CombatOutcome combatOutcome)
    {
        switch (combatOutcome)
        {
            case CombatOutcome.Miss:
                return "Miss";
            case CombatOutcome.Grace:
                return "Grace";
            case CombatOutcome.NormalHit:
                return "Hit";
            case CombatOutcome.Critical:
                return "Critical Hit";
            default:
                return "";
        }
    }

    private void EndContestedSkillcheck(List<CombatOutcome> combatOutcomes)
    {
        if (closeAfterCompleted)
        {
            foreach (var item in skillcheckTarget)
            {
                item.gameObject.SetActive(false);
            }

            parent.SetActive(false);
        }
        OnCombatOutcomesDecided?.Invoke(combatOutcomes);
    }
}

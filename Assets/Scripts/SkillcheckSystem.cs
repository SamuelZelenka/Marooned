using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillcheckSystem : MonoBehaviour
{
    const int numberOfCoins = 4;

    public enum CombatOutcome { Miss, Grace, NormalHit, Critical }

    public delegate void CombatOutcomeHandler(CombatOutcome combatOutcome);
    public static CombatOutcomeHandler OnCombatOutcomeDecided;

    [Header("References")]
    //[SerializeField] GameObject[] engagerCoins = null;

    [SerializeField] SkillcheckCoin[] attackerCoins = new SkillcheckCoin[numberOfCoins], defenderCoins = new SkillcheckCoin[numberOfCoins];
    [SerializeField] Text attackerStartValueText = null, attackerEndValueText = null;
    [SerializeField] Text defenderStartValueText = null, defenderEndValueText = null;
    [SerializeField] Text outcomeText = null;

    int attackerEndValue = 0, defenderEndValue = 0;

    public void DebugStartContested()
    {
        StartContestedSkillcheck(Random.Range(0, 10), Random.Range(0, 10));
    }

    public void StartContestedSkillcheck(int attackerStartValue, int defenderStartValue)
    {
        outcomeText.text = "";

        this.attackerStartValueText.text = attackerStartValue.ToString();
        this.defenderStartValueText.text = defenderStartValue.ToString();

        attackerEndValue = attackerStartValue;
        defenderEndValue = defenderStartValue;

        attackerEndValueText.text = attackerEndValue.ToString();
        defenderEndValueText.text = defenderEndValue.ToString();

        StartCoroutine(PerformContestedSkillcheck());
    }

    IEnumerator PerformContestedSkillcheck()
    {
        foreach (var item in attackerCoins)
        {
            item.TurnCoinToUndecided();
        }

        foreach (var item in defenderCoins)
        {
            item.TurnCoinToUndecided();
        }

        yield return new WaitForSeconds(0.1f);

        int attackerSuccesses = 0;

        for (int i = 0; i < numberOfCoins; i++)
        {
            bool attackerSuccess = SkillcheckCoin.FlipCoin();
            if (attackerSuccess)
            {
                attackerSuccesses++;
            }

            bool defenderSuccess = SkillcheckCoin.FlipCoin();

            attackerEndValue = attackerSuccess ? attackerEndValue + 1 : attackerEndValue - 1;
            defenderEndValue = defenderSuccess ? defenderEndValue + 1 : defenderEndValue - 1;

            StartCoroutine(attackerCoins[i].VisualizeCoinFlip(attackerSuccess));
            yield return defenderCoins[i].VisualizeCoinFlip(defenderSuccess);

            //Write text
            attackerEndValueText.text = attackerEndValue.ToString();
            defenderEndValueText.text = defenderEndValue.ToString();
        }
        DecideContestedOutcome(attackerSuccesses);
    }

    private void DecideContestedOutcome(int attackerSuccesses)
    {
        CombatOutcome combatOutcome = CombatOutcome.Miss;
        if (attackerSuccesses == numberOfCoins) //All successes = crit always
        {
            combatOutcome = CombatOutcome.Critical;
        }
        else if (attackerSuccesses == 0) //All misses = miss always
        {
            combatOutcome = CombatOutcome.Miss;
        }
        else
        {
            if (attackerEndValue >= defenderEndValue)
            {
                combatOutcome = attackerSuccesses == 1 ? CombatOutcome.Grace : CombatOutcome.NormalHit;
            }
        }
        outcomeText.text = OutcomeString(combatOutcome);

        OnCombatOutcomeDecided?.Invoke(combatOutcome);
    }

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
}

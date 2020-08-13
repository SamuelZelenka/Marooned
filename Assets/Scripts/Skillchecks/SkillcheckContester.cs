using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public class SkillcheckContester : MonoBehaviour
//{
//    [SerializeField] SkillcheckCoin[] coins = new SkillcheckCoin[SkillcheckSystem.NUMBEROFCOINS];
//    [SerializeField] Text startValueText = null, endValueText = null;
//    [SerializeField] Text outcomeText = null;
//    [SerializeField] Image portraitA = null, portraitB = null;

//    public bool done = false;


//    private int endValue;
//    public int EndValue
//    {
//        get => endValue;
//        set
//        {
//            endValue = value;
//            endValueText.text = value.ToString();
//        }
//    }

//    public void SetupSkillcheck(int startValue, Sprite portraitSprite)
//    {
//        startValueText.text = startValue.ToString();
//        EndValue = startValue;
//        portraitA.sprite = portraitSprite;
//        portraitB.sprite = portraitSprite;

//        foreach (var item in coins)
//        {
//            item.TurnCoinToUndecided();
//        }
//        outcomeText.text = "";
//    }

//    public static List<bool> GetCoinflipResults()
//    {
//        List<bool> coinflipResults = new List<bool>();
//        for (int i = 0; i < SkillcheckSystem.NUMBEROFCOINS; i++)
//        {
//            coinflipResults.Add(SkillcheckCoin.GetCoinflipResult());
//        }
//        return coinflipResults;
//    }


//    public IEnumerator FlipCoins(List<bool> wins)
//    {
//        done = false;
//        for (int i = 0; i < coins.Length; i++)
//        {
//            yield return coins[i].VisualizeCoinFlip(wins[i]);
//            if (wins[i])
//            {
//                EndValue++;
//            }
//            else
//            {
//                EndValue--;
//            }
//        }
//        done = true;
//    }

//    public void SetOutcomeText(string text) => outcomeText.text = text;
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeManager : MonoBehaviour
{
    static int[] BASECARDVALUES = new int[] { -2, -1, 0, 1, 1, 1, 2, 2 };
    const int DIFFICULTYCARDVALUE = 3;
    List<int> cardValues;
    [SerializeField] ChallengeCard[] challengeCards = null;
    [SerializeField] Text scoreText = null;
    [SerializeField] Text cardNumbersExplanation = null;

    [SerializeField] Text challengeDescriptionText = null;

    [SerializeField] Text criticalSuccessEffectText = null;
    [SerializeField] Text successEffectText = null;
    [SerializeField] Text failEffectText = null;
    [SerializeField] Text criticalFailEffectText = null;


    Challenge activeChallenge;
    int activeCharacterSkillLevel;

    int score;
    int Score
    {
        get => score;
        set
        {
            score = value;
            scoreText.text = $"{value}";
        }
    }

    const int FAILREQUIREMENT = 0;
    const int SUCCESSREQUIREMENT = 1;
    const int CRITICALSUCCESSREQUIREMENT = 2;


    bool challengeStarted = false;

    [SerializeField] bool debugGivenCards = false;

    public void StartChallenge(Challenge challenge)
    {
        Score = 0;
        foreach (var card in challengeCards)
        {
            card.ResetCard();
            card.gameObject.SetActive(false);
        }

        activeChallenge = challenge;
        challengeStarted = false;
        switch (challenge.challengeType)
        {
            case Challenge.Type.SkillcheckChallenge:
                //Start skillcheck challenge
                SetupTexts();
                CreateBaseCards(challenge.challengeLevel);
                CreateNegativeDifficultyCards();
                DisplayCardNumbers();
                break;
            case Challenge.Type.Combat:
                //Start combat
                break;
        }
    }

    private void SetupTexts()
    {
        challengeDescriptionText.text = activeChallenge.description;
        criticalSuccessEffectText.text = activeChallenge.criticalSuccessEffectText;
        successEffectText.text = activeChallenge.successEffectText;
        failEffectText.text = activeChallenge.failEffectText;
        criticalFailEffectText.text = activeChallenge.criticalFailEffectText;
    }

    private void CreateBaseCards(int sets)
    {
        //Add cards with values
        cardValues = new List<int>();
        for (int i = 0; i < sets; i++)
        {
            cardValues.AddRange(BASECARDVALUES);
        }
    }

    private void CreateNegativeDifficultyCards()
    {
        int cards = GetNumberOfNegativeDifficultyCards();
        for (int i = 0; i < cards; i++)
        {
            cardValues.Add(-DIFFICULTYCARDVALUE);
        }
    }

    private int GetNumberOfNegativeDifficultyCards() => activeChallenge.challengeLevel * activeChallenge.challengeLevel;
    private int GetNumberOfPositiveDifficultyCards() => activeCharacterSkillLevel * activeCharacterSkillLevel;
    private int GetNumberOfBaseCards(int valueOfCard)
    {
        int numbersFound = 0;
        foreach (var baseCard in BASECARDVALUES)
        {
            if (baseCard == valueOfCard)
                numbersFound++;
        }
        numbersFound *= activeChallenge.challengeLevel;
        return numbersFound;
    }


    //Show number of cards of categories
    private void DisplayCardNumbers()
    {
        cardNumbersExplanation.text =
            $"-3 cards: {GetNumberOfNegativeDifficultyCards()}\n" +
            $"-2 cards: {GetNumberOfBaseCards(-2)}\n" +
            $"-1 cards: {GetNumberOfBaseCards(-1)}\n" +
            $"0 cards: {GetNumberOfBaseCards(0)}\n" +
            $"+1 cards: {GetNumberOfBaseCards(1)}\n" +
            $"+2 cards: {GetNumberOfBaseCards(2)}\n" +
            $"+3 cards: {GetNumberOfPositiveDifficultyCards()}";
    }

    public void SetCharacter(int skillValue)
    {
        if (challengeStarted)
        {
            Debug.Log("Cannot change character when challenge is started");
            return;
        }
        activeCharacterSkillLevel = skillValue;
        //Add cards dependent on player character skill
        int cards = GetNumberOfPositiveDifficultyCards();
        for (int i = 0; i < cards; i++)
        {
            cardValues.Add(DIFFICULTYCARDVALUE);
        }
        DisplayCardNumbers();
        GiveCards(skillValue);
    }

    private void GiveCards(int skillValue)
    {
        int totalValueOfCardsGiven = 0;
        for (int i = 0; i < skillValue; i++)
        {
            challengeCards[i].gameObject.SetActive(true);
            int index = Random.Range(0, cardValues.Count);
            challengeCards[i].SetValue(cardValues[index], TurnCard);
            totalValueOfCardsGiven += cardValues[index];
            cardValues.RemoveAt(index);
        }

        if (debugGivenCards)
            DebugValueOfCardsGiven(totalValueOfCardsGiven);
    }

    private void DebugValueOfCardsGiven(int value)
    {
        Debug.Log(value);
        if (value >= FAILREQUIREMENT)
        {
            if (value >= SUCCESSREQUIREMENT)
            {
                if (value >= CRITICALSUCCESSREQUIREMENT)
                    Debug.Log("Critical Success");
                else
                    Debug.Log("Success");
            }
            else
                Debug.Log("Fail");
        }
        else
            Debug.Log("Critical Fail");
    }

    public void TurnCard(int valueOfCard)
    {
        if (!challengeStarted)
            challengeStarted = true;
        Score += valueOfCard;
    }

    private void VisualizeState()
    {

    }
}



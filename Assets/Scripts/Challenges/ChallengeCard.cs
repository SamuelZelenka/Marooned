using UnityEngine;
using UnityEngine.UI;

public class ChallengeCard : MonoBehaviour
{
    static int[] NUMBERS = new int[] { -3, -2, -1, 0, 1, 2, 3 };

    public delegate void CardHandler(int value);
    public event CardHandler OnCardClicked;

    [SerializeField] Sprite[] card = new Sprite[7];
    [SerializeField] Image cardFront = null;
    [SerializeField] Sprite cardBack = null;

    public int Value { private set; get; }

    public void ResetCard() => cardFront.sprite = cardBack;

    public void SetValue(int newValue, CardHandler onCardClickedMethod)
    {
        Value = newValue;
        OnCardClicked += onCardClickedMethod;
    }

    public void SendValueOnClick()
    {
        OnCardClicked?.Invoke(Value);
        OnCardClicked = null;
        cardFront.sprite = NumberToSprite(Value);
    }

    private Sprite NumberToSprite(int number)
    {
        for (int i = 0; i < NUMBERS.Length; i++)
        {
            if (number == NUMBERS[i])
                return card[i];
        }
        throw new System.ArgumentOutOfRangeException($"Sprite corresponding to number '{number}' not found");
    }
}
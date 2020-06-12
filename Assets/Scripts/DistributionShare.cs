using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistributionShare : MonoBehaviour
{
    public Character character;
    public Text characterName;
    public Image characterPortrait;

    public Slider slider;

    public Text shareText;
    public Text currentStat;
    public Text statChange;
    public Text newStat;

    public int totalShare;
    public float shareFactor;

    int resourcesConsumed;

    int equalSharePercentage;

    bool isCaptainShare;

    public void Setup(Character character, int totalShare, int numberOfShares, bool isCaptainShare)
    {
        //Setup
        this.transform.localScale = new Vector3(1, 1, 1);
        this.character = character;
        characterName.text = character.characterName;
        characterPortrait.sprite = character.portrait;

        //Share values
        this.totalShare = totalShare;
        equalSharePercentage = Mathf.RoundToInt(100 / numberOfShares);
        shareFactor = Utility.PercentageToFactor(equalSharePercentage);

        //Slider and calculation
        this.isCaptainShare = isCaptainShare;
        slider.interactable = !isCaptainShare;
        SetSliderValue(equalSharePercentage); //This will automatically update the values in SetSharePercentage and then update stats
    }

    private void UpdateStats()
    {
        float oldValue = character.resources.Loyalty;
        float newValue = GetCharacterResourceAfterAdd();
        float change = newValue - oldValue;

        string oldValueString = "";
        string newValueString = "";
        string changeString = "";

        oldValueString = Utility.FactorToPercentageText(oldValue);
        newValueString = Utility.FactorToPercentageText(newValue);
        changeString = Utility.FactorToPercentageText(change);

        shareText.text = Utility.FactorToPercentageText(shareFactor) + " (" + resourcesConsumed + ")";

        currentStat.text = oldValueString;
        newStat.text = newValueString;
        statChange.text = changeString;
    }

    public void SetSharePercentage(float value)
    {
        if (character)
        {
            float oldPercentage = Utility.FactorToPercentage(shareFactor);

            shareFactor = Utility.PercentageToFactor(Mathf.RoundToInt(value));
            resourcesConsumed = Mathf.RoundToInt(totalShare * shareFactor);
            UpdateStats();

            float newPercentage = Utility.FactorToPercentage(shareFactor);


            if (!isCaptainShare)
            {
                DistributionSystem.OnSharesChanged?.Invoke();
            }
        }
    }

    public void SetSliderValue(float value)
    {
        slider.value = value;
    }

    public void ChangeSliderValue(float change)
    {
        slider.value += change;
    }

    public void ConfirmShare()
    {
        character.resources.Loyalty = Mathf.RoundToInt(GetCharacterResourceAfterAdd());
    }

    private float GetCharacterResourceAfterAdd()
    {
        return Mathf.Min(1, character.resources.Loyalty + shareFactor - Utility.PercentageToFactor(equalSharePercentage));
    }
}

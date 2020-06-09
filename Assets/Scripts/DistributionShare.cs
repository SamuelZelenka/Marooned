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
    public Image shareTypeImage;
    public Image statTypeImage;

    public Text shareText;
    public Text currentStat;
    public Text statChange;
    public Text newStat;

    CharacterResources.ResourceType resourceType;

    public int totalShare;
    public float shareFactor;
    public int shareNumber;


    public void Setup(Character character, int totalShare, int numberOfShares, Sprite shareTypeSprite, Sprite statChangeSprite, CharacterResources.ResourceType resourceType)
    {
        this.character = character;
        //characterName = character.name;
        //characterPortrait = character.portrait;

        this.totalShare = totalShare;

        int equalSharePercentage = Mathf.RoundToInt(100 / numberOfShares);
        slider.value = equalSharePercentage;

        shareTypeImage.sprite = shareTypeSprite;
        statTypeImage.sprite = statChangeSprite;

        this.resourceType = resourceType;

        UpdateStats();
    }

    public void UpdateStats()
    {
        shareText.text = Utility.FactorToPercentageText(shareFactor) + "(" + shareNumber + ")";
        currentStat.text = Utility.FactorToPercentageText(character.resources.GetResource(resourceType));
        statChange.text = "+" + Utility.FactorToPercentageText(shareFactor);
        newStat.text = Mathf.Min(Utility.FactorToPercentage(character.resources.GetResource(resourceType)) + Utility.FactorToPercentage(shareFactor), 100).ToString();
    }

    public void SetSharePercentage(int wholePercentage)
    {
        shareFactor = Utility.PercentageToFactor(wholePercentage);
        shareNumber = Mathf.RoundToInt(totalShare * shareFactor);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistributionSystem : MonoBehaviour
{
    public GameObject mainPanel;
    public Text distributionTitle;

    public DistributionShare distributionPrefab;
    public Transform distributionParent;

    public List<DistributionShare> shares = new List<DistributionShare>();

    public Sprite coin;
    public Sprite rawFood;
    public Sprite medicalSupplies;

    public Sprite hunger;
    public Sprite vitality;
    public Sprite loyalty;

    public void Setup(List<Character> charactersToShare, int totalShare,  int numberOfShares, CharacterResources.ResourceType resourceTypeToShare)
    {
        mainPanel.SetActive(true);

        Sprite resourceSprite;
        Sprite statsprite;

        GetSpritesForDistributionPanels(resourceTypeToShare, out resourceSprite, out statsprite);

        for (int i = 0; i < charactersToShare.Count; i++)
        {
            DistributionShare newShareObject = Instantiate(distributionPrefab);
            newShareObject.transform.SetParent(distributionParent);
            newShareObject.Setup(charactersToShare[i], totalShare, numberOfShares, resourceSprite, statsprite, resourceTypeToShare);
        }
    }

    private void GetSpritesForDistributionPanels(CharacterResources.ResourceType resourceTypeToShare, out Sprite distributedSprite, out Sprite affectedStatSprite)
    {
        switch (resourceTypeToShare)
        {
            case CharacterResources.ResourceType.Vitality:
                distributedSprite = medicalSupplies;
                affectedStatSprite = vitality;
                break;
            case CharacterResources.ResourceType.Hunger:
                distributedSprite = rawFood;
                affectedStatSprite = hunger;
                break;
            case CharacterResources.ResourceType.Loyalty:
                distributedSprite = coin;
                affectedStatSprite = loyalty;
                break;
            default:
            case CharacterResources.ResourceType.Energy:
            case CharacterResources.ResourceType.Hygiene:
                distributedSprite = null;
                affectedStatSprite = null;
                Debug.LogError("Not set up sprites for distribution");
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HarborView : MonoBehaviour
{
    [SerializeField] Text poiTextTitle = null;
    [SerializeField] ResourceInteractionController merchantController = null;
    [SerializeField] PartyMember[] partyMembers = null;
    [SerializeField] Transform partyTransform = null;

    [SerializeField] GameObject tavernButton = null;
    [SerializeField] GameObject merchantButton = null;


    public void Setup(Harbor harbor)
    {
        poiTextTitle.text = harbor.name;
        merchantController.Setup(harbor);
        tavernButton.SetActive(harbor.hasTavern);
        merchantButton.SetActive(harbor.hasMerchant);

        if (harbor.hasTavern)
        {
            for (int i = 0; i < HexGridController.player.Crew.Count; i++)
            {
                partyMembers[i].character = HexGridController.player.Crew[i];
            }
            foreach (var partyMember in partyMembers)
            {
                partyMember.OnButtonClick += harbor.FeedCharacter;
                partyMember.OnButtonClick += UpdateTavernCrew;
            }
            UpdateTavernCrew(null);
        }
    }

    public void UpdateTavernCrew(Character character)
    {
        SyncPortraitCount();
        foreach (var partyMember in partyMembers)
        {
            if (partyMember.character != null)
            {
                partyMember.UpdateUI(CharacterResourceType.Vitality, CharacterResourceType.Hunger);
            }
        }
    }

    private void SyncPortraitCount()
    {
        for (int i = 0; i < HexGridController.player.Crew.Count; i++)
        {
            partyTransform.GetChild(i).gameObject.SetActive(true);
            partyMembers[i].SetCharacter(HexGridController.player.Crew[i]);
        }
        if (partyTransform.childCount > HexGridController.player.Crew.Count)
        {
            for (int j = HexGridController.player.Crew.Count; j < partyTransform.childCount; j++)
            {
                partyTransform.GetChild(j).gameObject.SetActive(false);
            }
        }
    }
}

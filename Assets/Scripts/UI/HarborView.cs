using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HarborView : MonoBehaviour
{
    [Header("Misc references")]
    [SerializeField] ResourceInteractionController merchantController = null;

    [Header("Main panel")]
    [SerializeField] Text poiTextTitle = null;
    [SerializeField] GameObject tavernButton = null;
    [SerializeField] GameObject merchantButton = null;

    [Header("Tavern")]
    [SerializeField] PartyMember[] partyMembers = null;
    [SerializeField] Transform partyTransform = null;
    [SerializeField] CharacterDetailsView[] recruitablePirateStatsViews = null;
    [SerializeField] GameObject[] recruitCharacterObjects = null;
    [SerializeField] Button[] tavernFeedButtons = null;
    [SerializeField] Button tavernRecruitButton = null;


    Harbor latestHarbor;

    private void OnDisable()
    {
        if (latestHarbor != null)
        {
            foreach (var partyMember in partyMembers)
            {
                partyMember.OnButtonClick -= latestHarbor.FeedCharacter;
                partyMember.OnButtonClick -= UpdateTavernCrew;
            }
            latestHarbor.OnHarborChanged -= HarborUpdated;
            HexGridController.player.PlayerData.OnGoldChanged -= UpdateTavernButtonStatuses;
        }
        latestHarbor = null;
    }

    public void Setup(Harbor harbor)
    {
        latestHarbor = harbor;
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
            harbor.OnHarborChanged += HarborUpdated;
            HexGridController.player.PlayerData.OnGoldChanged += UpdateTavernButtonStatuses;

            UpdateTavernCrew(null);
            UpdateTavernRecruitable();
            UpdateTavernButtonStatuses();
        }
    }

    private void HarborUpdated(Harbor harbor)
    {
        UpdateTavernRecruitable();
    }

    private void UpdateTavernCrew(Character character)
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

    private void UpdateTavernRecruitable()
    {
        foreach (var item in recruitCharacterObjects)
        {
            item.SetActive(latestHarbor.recruitableCharacter != null);
        }
        if (latestHarbor.recruitableCharacter != null)
        {
            foreach (var item in recruitablePirateStatsViews)
            {
                item.UpdateValues(latestHarbor.recruitableCharacter);
            }
        }
    }

    private void UpdateTavernButtonStatuses()
    {
        foreach (var item in tavernFeedButtons)
        {
            item.interactable = latestHarbor.CanBuyFood(out int foodCost);
        }
        tavernRecruitButton.interactable = latestHarbor.IsRecruitable(out int characterCost);
    }

    public void RecruitCharacter() => latestHarbor.RecruitCharacter();
}

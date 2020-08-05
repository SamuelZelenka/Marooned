using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class CombatUIView : MonoBehaviour
{
    [Header("Player Party")]
    [SerializeField] List<PartyMember> partyMembers = null;
    [SerializeField] Transform partyTransform = null;

    [Header("Selected Character")]
    [SerializeField] CharacterDetailsView selectedCharacter = null;

    [Header("Active Character")]
    [SerializeField] CharacterDetailsView activeCharacter = null;
    [SerializeField] AbilityUIController abilityUI = null;

    [Header("Timeline")]
    [SerializeField] CharacterPortrait currentCharacter = null;
    [SerializeField] List<CharacterPortrait> upcomingCharacters = new List<CharacterPortrait>();

    [Header("CombatLog")]
    [SerializeField] List<string> combatLog = new List<string>();
    public CombatLog combatLogDisplay = null;

    private void OnEnable()
    {
        HexUnit.OnAnyUnitMoved += UnitMoved;
        HexGridController.OnCellSelected += CellSelected;
        CombatTurnSystem.OnTurnBegining += TurnStarted;
        //CharacterData.OnEffectChanged += UpdateAllCharacters;
        //CharacterData.OnResourceChanged += UpdateAllCharacters;
        //CharacterData.OnStatChanged += UpdateAllCharacters;
    }
    private void OnDisable()
    {
        HexUnit.OnAnyUnitMoved -= UnitMoved;
        HexGridController.OnCellSelected -= CellSelected;
        CombatTurnSystem.OnTurnBegining -= TurnStarted;
        //CharacterData.OnEffectChanged -= UpdateAllCharacters;
        //CharacterData.OnResourceChanged -= UpdateAllCharacters;
        //CharacterData.OnStatChanged -= UpdateAllCharacters;
    }
    //INFO 
    // UPDATING ALL CHARACTERS EVERY TIME ANY CHARACTER CHANGES ANY STAT SEEMS UNREASONABLE
    //SIMON 

    private void UpdateTimeline()
    {
        List<Character> turnOrder = CombatTurnSystem.TurnOrder.ToList();

        currentCharacter.UpdatePortrait(HexGridController.ActiveCharacter);
        for (int i = 0; i < turnOrder.Count; i++)
        {
            upcomingCharacters[i].UpdatePortrait(turnOrder[i]);
        }
        if (upcomingCharacters.Count > turnOrder.Count)
        {
            for (int j = turnOrder.Count; j < upcomingCharacters.Count; j++)
            {
                upcomingCharacters[j].gameObject.SetActive(false);
            }
        }
    }
    private void UpdateAllCharacters()
    {
        if (HexGridController.SelectedCharacter)
        {
            selectedCharacter.UpdateValues(HexGridController.SelectedCharacter);
        }
        if (HexGridController.ActiveCharacter != null)
        {
            activeCharacter.UpdateValues(HexGridController.ActiveCharacter);
            abilityUI.UpdateUI();
        }
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
        foreach (PartyMember member in partyMembers)
        {
            if (member.character != null)
            {
                member.UpdateUI(CharacterResourceType.Vitality, CharacterResourceType.Loyalty, CharacterResourceType.Energy);
            }
        }
    }
    private void UnitMoved(HexUnit unit) => UpdateAllCharacters();
    private void CellSelected(HexCell cell) => UpdateAllCharacters();
    private void TurnStarted(Character character)
    {
        UpdateAllCharacters();
        UpdateTimeline();
    }
}
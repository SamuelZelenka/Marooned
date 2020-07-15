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
    [SerializeField] CharacterSelection selectedCharacter = null;
    [SerializeField] AbilityUIController abilityUI = null;

    [Header("Active Character")]
    [SerializeField] CharacterSelection activeCharacter = null;

    [Header("Timeline")]
    [SerializeField] CharacterPortrait currentCharacter = null;
    [SerializeField] List<CharacterPortrait> upcomingCharacters = new List<CharacterPortrait>();

    [Header("CombatLog")]
    [SerializeField] List<string> combatLog = new List<string>();
    public CombatLog combatLogDisplay = null;

    private void OnEnable()
    {
        HexUnit.OnUnitMoved += UnitMoved;
        HexGridController.OnCellSelected += CellSelected;
        CombatTurnSystem.OnTurnBegining += TurnStarted;
        CharacterData.OnEffectChanged += UpdateAllCharacters;
        CharacterData.OnResourceChanged += UpdateAllCharacters;
        CharacterData.OnStatChanged += UpdateAllCharacters;
    }
    private void OnDisable()
    {
        HexUnit.OnUnitMoved -= UnitMoved;
        HexGridController.OnCellSelected -= CellSelected;
        CombatTurnSystem.OnTurnBegining -= TurnStarted;
        CharacterData.OnEffectChanged -= UpdateAllCharacters;
        CharacterData.OnResourceChanged -= UpdateAllCharacters;
        CharacterData.OnStatChanged -= UpdateAllCharacters;
    }

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
        foreach (PartyMember member in partyMembers)
        {
            if (member.character != null)
            {
                member.UpdateUI();
            }
        }
        if (HexGridController.SelectedCharacter)
        {
            selectedCharacter.UpdateUI(HexGridController.SelectedCharacter);
        }
        if (HexGridController.ActiveCharacter != null)
        {
            activeCharacter.UpdateUI(HexGridController.ActiveCharacter);
            abilityUI.UpdateUI();
        }
        for (int i = 0; i < HexGridController.player.Crew.Count; i++)
        {
            partyTransform.GetChild(i).gameObject.SetActive(true);
            partyMembers[i].SetCharacter(HexGridController.player.Crew[i]);
            partyMembers[i].UpdateUI();
        }
        if (partyTransform.childCount > HexGridController.player.Crew.Count)
        {
            for (int j = HexGridController.player.Crew.Count; j < partyTransform.childCount; j++)
            {
                partyTransform.GetChild(j).gameObject.SetActive(false);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIController : MonoBehaviour
{
    [Header("Player Party")]
    [SerializeField] List<PartyMember> partyMembers = null;
    [SerializeField] Transform partyTransform = null;

    [Header("Selected Character")]
    [SerializeField] CharacterSelection selectedCharacter = null;

    [Header("Active Character")]
    [SerializeField] CharacterSelection activeCharacter = null;
    [SerializeField] List<Image> abilities = new List<Image>();

    [Header("Timeline")]
    [SerializeField] Image currentCharacter = null;
    [SerializeField] List<Image> upcomingCharacters = new List<Image>();

    [Header("CombatLog")]
    [SerializeField] List<string> combatLog = new List<string>();
    public CombatLog combatLogDisplay = null;

    private void OnEnable()
    {
        HexUnit.OnUnitMoved += UnitMoved;
        HexGridController.OnCellSelected += CellSelected;
        CombatTurnSystem.OnTurnEnding += TurnStarted;
        CharacterData.OnEffectChanged += UpdateAllCharacters;
        CharacterData.OnResourceChanged += UpdateAllCharacters;
        CharacterData.OnStatChanged += UpdateAllCharacters;
    }
    private void OnDisable()
    {
        HexUnit.OnUnitMoved -= UnitMoved;
        HexGridController.OnCellSelected -= CellSelected;
        CombatTurnSystem.OnTurnEnding -= TurnStarted;
        CharacterData.OnEffectChanged -= UpdateAllCharacters;
        CharacterData.OnResourceChanged -= UpdateAllCharacters;
        CharacterData.OnStatChanged -= UpdateAllCharacters;

    }

    public void UpdateTimeline(List<Character> turnOrder)
    {
        currentCharacter.sprite = HexGridController.ActiveCharacter.characterData.portrait;
        for (int i = 0; i < turnOrder.Count; i++)
        {
            upcomingCharacters[i].sprite = turnOrder[i].characterData.portrait;
        }
        if (upcomingCharacters.Count > turnOrder.Count)
        {
            for (int j = turnOrder.Count; j < upcomingCharacters.Count; j++)
            {
                upcomingCharacters[j].gameObject.SetActive(false);
            }
        }
    }
    public void UpdateAllCharacters()
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

            for (int i = 0; i < HexGridController.ActiveCharacter.Abilities.Count; i++)
            {
                abilities[i].sprite = HexGridController.ActiveCharacter.Abilities[i].abilitySprite;
            }
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
    private void TurnStarted(Character character) => UpdateAllCharacters();
}
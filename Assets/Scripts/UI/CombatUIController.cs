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
    [SerializeField] List<string> log = new List<string>();
    [SerializeField] Text logDisplay = null;

    private void OnEnable()
    {
        HexUnit.OnUnitMoved += UnitMoved;
        HexGridController.OnCellSelected += CellSelected;
        CombatTurnSystem.OnTurnEnding += TurnStarted;
        CombatSystem.OnAbilityUsed += UpdateAllCharacters;
    }
    private void OnDisable()
    {
        HexUnit.OnUnitMoved -= UnitMoved;
        HexGridController.OnCellSelected -= CellSelected;
        CombatTurnSystem.OnTurnEnding -= TurnStarted;
        CombatSystem.OnAbilityUsed -= UpdateAllCharacters;

    }
    public void UpdateCombatLog(string newLog)
    {
        log.Add(log.Count + " " + newLog);
        logDisplay.text = "";
        for (int i = log.Count - 1; i >= 0; i--)
        {
            logDisplay.text += "\n-" + log[i];
        }
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
    public void UpdateCrewDisplay(List<Character> playerCrew)
    {
        for (int i = 0; i < playerCrew.Count; i++)
        {
            partyTransform.GetChild(i).gameObject.SetActive(true);
            partyMembers[i].SetCharacter(playerCrew[i]);
            partyMembers[i].UpdateUI();
        }
        if (partyTransform.childCount > playerCrew.Count)
        {
            for (int j = playerCrew.Count; j < partyTransform.childCount; j++)
            {
                partyTransform.GetChild(j).gameObject.SetActive(false);
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
        if (HexGridController.SelectedCharacter != null)
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
    }
    private void UnitMoved(HexUnit unit) => UpdateAllCharacters();
    private void CellSelected(HexCell cell) => UpdateAllCharacters();
    private void TurnStarted(Character character) => UpdateAllCharacters();
}
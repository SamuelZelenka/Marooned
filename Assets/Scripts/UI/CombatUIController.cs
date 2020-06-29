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

    private void OnEnable()
    {
        CombatTurnSystem.OnTurnBegining += SelectActiveCharacter;
        PlayerInput.OnUnitSelected += SelectCharacter;
    }
    private void OnDisable()
    {
        CombatTurnSystem.OnTurnBegining -= SelectActiveCharacter;
        PlayerInput.OnUnitSelected -= SelectCharacter;
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
            member.UpdateUI();
        }
        selectedCharacter.UpdateUI();
        activeCharacter.UpdateUI();
    }

    public void SelectCharacter(HexUnit unit)
    {
        selectedCharacter.SelectCharacter(unit);
    }

    public void SelectActiveCharacter(Character character)
    {
        activeCharacter.SelectCharacter(character.Location.Unit);
        for (int i = 0; i < character.Abilities.Count; i++)
        {
            abilities[i].sprite = character.Abilities[i].abilitySprite;
        }
    }
}

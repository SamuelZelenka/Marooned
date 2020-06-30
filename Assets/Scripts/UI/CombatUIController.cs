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
        HexUnit.OnUnitMoved += UnitMoved;
    }
    private void OnDisable()
    {
        HexUnit.OnUnitMoved -= UnitMoved;
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
    private void UnitMoved(HexUnit unit)
    {
        UpdateAllCharacters();
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
        selectedCharacter.UpdateUI(HexGridController.SelectedCharacter);
        activeCharacter.UpdateUI(HexGridController.activeCharacter);

        for (int i = 0; i < HexGridController.activeCharacter.Abilities.Count; i++)
        {
            abilities[i].sprite = HexGridController.activeCharacter.Abilities[i].abilitySprite;
        }
    }
}
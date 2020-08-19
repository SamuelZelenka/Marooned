using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class CombatUIView : MonoBehaviour
{
    [Header("Player Crew")]
    [SerializeField] CrewDisplay crewDisplay = null;

    [Header("Selected Character")]
    [SerializeField] CharacterView selectedCharacter = null;

    [Header("Active Character")]
    [SerializeField] CharacterView activeCharacter = null;
    [SerializeField] AbilityUIController abilityUI = null;

    [Header("Timeline")]
    [SerializeField] List<CharacterView> upcomingCharacters = new List<CharacterView>();

    private void OnEnable()
    {
        HexGridController.OnCharacterSelected += UpdateSelectedCharacter;
        HexGridController.OnActiveCharacterChanged += UpdateActiveCharacter;
    }
    private void OnDisable()
    {
        HexGridController.OnCharacterSelected -= UpdateSelectedCharacter;
        HexGridController.OnActiveCharacterChanged -= UpdateActiveCharacter;
    }

    private void UpdateTimeline()
    {
        List<Character> turnOrder = CombatTurnSystem.TurnOrder.ToList();

        foreach (var upcomingCharacter in upcomingCharacters)
        {
            upcomingCharacter.gameObject.SetActive(false);
        }
        for (int i = 0; i < turnOrder.Count; i++)
        {
            upcomingCharacters[i].gameObject.SetActive(true);
            upcomingCharacters[i].SetCharacter(turnOrder[i]);
        }
    }

    private void UpdateActiveCharacter(Character character)
    {
        activeCharacter.SetCharacter(character);
        abilityUI.UpdateUI(character);
        UpdateTimeline();
    }

    private void UpdateSelectedCharacter(Character character) => selectedCharacter.SetCharacter(character);

    public void UpdateCrew(List<Character> crew) => crewDisplay.UpdateCrew(crew);
}
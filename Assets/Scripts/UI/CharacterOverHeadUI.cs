using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOverHeadUI : MonoBehaviour
{
    [SerializeField] Character character;
    [SerializeField] Bar vitality;
    [SerializeField] Bar loyalty;

    private void OnEnable()
    {
        CombatTurnSystem.OnTurnBegining += TurnStarted;
        CombatSystem.OnAbilityUsed += UpdateUI;
    }
    private void OnDisable()
    {
        CombatTurnSystem.OnTurnBegining -= TurnStarted;
        CombatSystem.OnAbilityUsed -= UpdateUI;
    }

    public void UpdateUI()
    {
        vitality.SetCurrentValue(character.characterData.Vitality.CurrentValue);
        vitality.SetMaxValue(character.characterData.Vitality.maxValue);
        loyalty.SetCurrentValue(character.characterData.Loyalty.CurrentValue);
        loyalty.SetMaxValue(character.characterData.Loyalty.maxValue);
    }
    private void TurnStarted(Character character) => UpdateUI();
}

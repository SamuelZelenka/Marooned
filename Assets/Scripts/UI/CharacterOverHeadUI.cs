using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOverHeadUI : MonoBehaviour
{
    [SerializeField] Character character = null;
    [SerializeField] Bar vitality = null;
    [SerializeField] Bar loyalty = null;

    private void OnEnable()
    {
        CharacterData.OnResourceChanged += UpdateUI;
    }
    private void OnDisable()
    {
        CharacterData.OnResourceChanged -= UpdateUI;
    }

    public void UpdateUI()
    {
        vitality.SetCurrentValue(character.characterData.Vitality.CurrentValue);
        vitality.SetMaxValue(character.characterData.Vitality.maxValue);
        loyalty.SetCurrentValue(character.characterData.Loyalty.CurrentValue);
        loyalty.SetMaxValue(character.characterData.Loyalty.maxValue);
    }
}

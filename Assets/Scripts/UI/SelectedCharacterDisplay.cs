using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedCharacterDisplay : MonoBehaviour
{
    [SerializeField] Text characterName = null;
    [SerializeField] Image portrait = null;
    [SerializeField] Text loyalty = null;
    [SerializeField] Text strength = null;
    [SerializeField] Text stamina = null;
    [SerializeField] Text constitution = null;
    [SerializeField] Text agility = null;
    [SerializeField] Text toughness = null;
    [SerializeField] Text accuracy = null;
    [SerializeField] Bar vitality = null;
    [SerializeField] Bar energy = null;

    [SerializeField] List<Image> abilities = new List<Image>();

    public void UpdateUI(Character character)
    {
        characterName.text = character.characterData.characterName;
        portrait.sprite = character.characterData.portrait;
        loyalty.text = character.characterData.Loyalty.CurrentValue.ToString();
        strength.text = $"STR: {character.characterData.Strength}";
        stamina.text = $"STA: {character.characterData.Stamina}";
        constitution.text = $"CON: {character.characterData.Constitution}";
        agility.text = $"AGI: {character.characterData.Agility}";
        toughness.text = $"TOU: {character.characterData.Toughness}";
        accuracy.text = $"ACC: {character.characterData.Accuracy}";

        vitality.SetMaxValue(character.characterData.Vitality.maxValue);
        vitality.SetCurrentValue(character.characterData.Vitality.CurrentValue);

        energy.SetMaxValue(character.characterData.Energy.maxValue);
        energy.SetCurrentValue(character.characterData.Energy.CurrentValue);



        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i].sprite = character.Abilities[i].abilitySprite;
        }
    }
}

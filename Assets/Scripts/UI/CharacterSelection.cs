using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] Text characterName = null;
    [SerializeField] Image portrait = null;

    [SerializeField] Text moves = null;

    [SerializeField] Text strength = null;
    [SerializeField] Text stamina = null;
    [SerializeField] Text constitution = null;
    [SerializeField] Text agility = null;
    [SerializeField] Text toughness = null;
    [SerializeField] Text accuracy = null;
    [SerializeField] Bar loyalty = null;
    [SerializeField] Bar vitality = null;
    [SerializeField] Bar energy = null;

    public void UpdateUI(Character character)
    {
        if (portrait != null)
        {
            portrait.sprite = character.characterData.portrait;
        }
        characterName.text = character.characterData.characterName;
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

        loyalty.SetMaxValue(character.characterData.Loyalty.maxValue);
        loyalty.SetCurrentValue(character.characterData.Loyalty.CurrentValue);


        if (character.playerControlled)
        {
            //disable abilities
            //  energy.gameObject.SetActive(true);
        }
        else
        {
            //  energy.gameObject.SetActive(false);

        }
    }
}

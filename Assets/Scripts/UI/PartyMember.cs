using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PartyMember : MonoBehaviour
{

    public Character character;
    [SerializeField] Text characterName = null;
    [SerializeField] Image portraitImage = null;
    [SerializeField] List<Image> effects = null;
    [SerializeField] Bar vitality = null;
    [SerializeField] Bar loyalty = null;
    [SerializeField] Bar energy = null;
    


    public void SetCharacter(Character character)
    {
        this.character = character;
    }
    public void UpdateUI()
    {
        characterName.text = character.characterData.characterName;
        portraitImage.sprite = character.characterData.portrait;

        vitality.SetCurrentValue(character.characterData.Vitality.CurrentValue);
        vitality.SetMaxValue(character.characterData.Vitality.maxValue);
        loyalty.SetCurrentValue(character.characterData.Loyalty.CurrentValue);
        loyalty.SetMaxValue(character.characterData.Loyalty.maxValue);
        energy.SetCurrentValue(character.characterData.Energy.CurrentValue);
        energy.SetMaxValue(character.characterData.Energy.maxValue);

        foreach (Image image in effects)
        {
            image.enabled = false;
        }
        for (int i = 0; i < character.characterData.activeEffects.Count; i++)
        {
            effects[i].sprite = character.characterData.activeEffects[i].effectSprite;
        }
    }
}
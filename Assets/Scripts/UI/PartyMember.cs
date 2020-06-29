using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PartyMember : MonoBehaviour
{
    [SerializeField] Text characterName = null;
    [SerializeField] Image portraitImage = null;
    [SerializeField] List<Image> buffs = null;
    [SerializeField] Bar vitality = null;
    [SerializeField] Bar energy = null;
    public void UpdateUI(Character character)
    {
        characterName.text = character.characterData.characterName;
        portraitImage.sprite = character.characterData.portrait;
        vitality.SetCurrentValue(character.characterData.Vitality.CurrentValue);
        vitality.SetMaxValue(character.characterData.Vitality.maxValue);
        vitality.SetCurrentValue(character.characterData.Energy.CurrentValue);
        vitality.SetMaxValue(character.characterData.Energy.maxValue);

        foreach (Image image in buffs)
        {
            image.enabled = false;
        }
        for (int i = 0; i < character.characterData.activeEffects.Count; i++)
        {
            buffs[i].sprite = character.characterData.activeEffects[i].effectSprite;
        }
    }
}
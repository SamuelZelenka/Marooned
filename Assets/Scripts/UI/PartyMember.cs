using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PartyMember : MonoBehaviour
{

    public Character character;
    [SerializeField] Text characterName = null;
    [SerializeField] Image portraitImage = null;
    [SerializeField] Image activeBorder = null;
    [SerializeField] Bar vitality = null;
    [SerializeField] Bar loyalty = null;
    [SerializeField] Bar energy = null;
    [SerializeField] MouseHoverImage prefab = null ;
    [SerializeField] Transform effectParent = null;

    List<MouseHoverImage> activeEffects = new List<MouseHoverImage>();

    public void SetCharacter(Character character)
    {
        this.character = character;
    }
    public void SelectThisCharacter()
    {
        HexGridController.SelectedCell = character.Location;
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

        if (HexGridController.ActiveCharacter == character)
        {
            activeBorder.gameObject.SetActive(true);
        }
        else
        {
            activeBorder.gameObject.SetActive(false);

        }

        if (SyncEffectLists())
        {
            for (int i = 0; i < character.characterData.activeEffects.Count; i++)
            {
                string effectDescription = character.characterData.activeEffects[i].Description;
                Sprite effectSprite = character.characterData.activeEffects[i].effectSprite;
                activeEffects[i].UpdateUI(effectDescription, effectSprite);
            }
        }

        bool SyncEffectLists()
        {
            if (activeEffects.Count < character.characterData.activeEffects.Count)
            {
                while (activeEffects.Count < character.characterData.activeEffects.Count)
                {
                    activeEffects.Add(Instantiate(prefab, effectParent));
                }
            }
            if (activeEffects.Count > character.characterData.activeEffects.Count)
            {
                while (activeEffects.Count > character.characterData.activeEffects.Count)
                {
                    activeEffects.RemoveAt(0);
                }
            }
            return activeEffects.Count == character.characterData.activeEffects.Count;
        }
    }


}
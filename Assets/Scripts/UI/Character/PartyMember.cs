using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PartyMember : MonoBehaviour
{
    public delegate void CharacterHandler(Character character);
    public CharacterHandler OnButtonClick;

    public Character character;
    [SerializeField] Text characterName = null;
    [SerializeField] CharacterPortrait portraitImage = null;
    [SerializeField] Image activeBorder = null;
    [SerializeField] Bar[] bars = null;
    [SerializeField] MouseHoverImage prefab = null ;
    [SerializeField] Transform effectParent = null;

    List<MouseHoverImage> activeEffects = new List<MouseHoverImage>();


    public void SetCharacter(Character character)
    {
        this.character = character;
    }

    public void UpdateUI(params CharacterResourceType[] characterResourceTypes)
    {
        characterName.text = character.characterData.CharacterName;
        portraitImage.UpdatePortrait(character);
        foreach (var bar in bars)
        {
            bar.gameObject.SetActive(false);
        }

        for (int i = 0; i < characterResourceTypes.Length && i < bars.Length; i++)
        {
            bars[i].gameObject.SetActive(true);
            CharacterData.Resource resource = character.characterData.GetResource(characterResourceTypes[i]);
            bars[i].SetMaxValue(resource.maxValue);
            bars[i].SetCurrentValue(resource.CurrentValue);
        }

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
                string effectDescription = character.characterData.activeEffects[i].GetDescription();
                Sprite effectSprite = character.characterData.activeEffects[i].EffectSprite;
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
                    Destroy(activeEffects[0].gameObject);
                    activeEffects.RemoveAt(0);
                }
            }
            return activeEffects.Count == character.characterData.activeEffects.Count;
        }
    }

    public void OnClick()
    {
        OnButtonClick?.Invoke(character);
    }
}
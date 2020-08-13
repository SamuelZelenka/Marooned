using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterOverHeadUI : MonoBehaviour
{
    [SerializeField] Character character = null;
    [SerializeField] Bar vitality = null;
    [SerializeField] Bar loyalty = null;
    [SerializeField] Text overheadText = null;

    [SerializeField] MouseHoverImage prefab = null;
    [SerializeField] Transform effectParent = null;

    List<MouseHoverImage> activeEffects = new List<MouseHoverImage>();

    private void OnEnable()
    {
        character.characterData.OnAnyResourceChanged += UpdateValues;
        character.characterData.OnEffectChanged += UpdateValues;
    }
    private void OnDisable()
    {
        character.characterData.OnAnyResourceChanged -= UpdateValues;
        character.characterData.OnEffectChanged -= UpdateValues;
    }

    public void UpdateValues(Character characterToUpdate) => UpdateValues(characterToUpdate.characterData);

    public void UpdateValues(CharacterData updatedData)
    {
        vitality.SetMaxValue(updatedData.Vitality.MaxValue);
        vitality.SetCurrentValue(updatedData.Vitality.CurrentValue);
        loyalty.SetMaxValue(updatedData.Loyalty.MaxValue);
        loyalty.SetCurrentValue(updatedData.Loyalty.CurrentValue);

        if (SyncEffectLists())
        {
            for (int i = 0; i < updatedData.activeEffects.Count; i++)
            {
                string effectDescription = updatedData.activeEffects[i].GetDescription();
                Sprite effectSprite = updatedData.activeEffects[i].EffectSprite;
                activeEffects[i].UpdateUI(effectDescription, effectSprite);
            }
        }

        bool SyncEffectLists()
        {
            if (activeEffects.Count < updatedData.activeEffects.Count)
            {
                while (activeEffects.Count < updatedData.activeEffects.Count)
                {
                    activeEffects.Add(Instantiate(prefab, effectParent));
                }
            }
            if (activeEffects.Count > updatedData.activeEffects.Count)
            {
                while (activeEffects.Count > updatedData.activeEffects.Count)
                {
                    Destroy(activeEffects[0].gameObject);
                    activeEffects.RemoveAt(0);
                }
            }
            return activeEffects.Count == updatedData.activeEffects.Count;
        }
    }

    public void SetOverheadText(string text) => overheadText.text = text;
}

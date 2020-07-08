using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOverHeadUI : MonoBehaviour
{
    [SerializeField] Character character = null;
    [SerializeField] Bar vitality = null;
    [SerializeField] Bar loyalty = null;

    [SerializeField] MouseHoverImage prefab = null;
    [SerializeField] Transform effectParent = null;

    List<MouseHoverImage> activeEffects = new List<MouseHoverImage>();

    private void OnEnable()
    {
        CharacterData.OnResourceChanged += UpdateUI;
        CombatTurnSystem.OnTurnEnding += UpdateUI;

    }
    private void OnDisable()
    {
        CharacterData.OnResourceChanged -= UpdateUI;
        CombatTurnSystem.OnTurnEnding -= UpdateUI;
    }

    public void UpdateUI(Character character) => UpdateUI();

    public void UpdateUI()
    {
        vitality.SetCurrentValue(character.characterData.Vitality.CurrentValue);
        vitality.SetMaxValue(character.characterData.Vitality.maxValue);
        loyalty.SetCurrentValue(character.characterData.Loyalty.CurrentValue);
        loyalty.SetMaxValue(character.characterData.Loyalty.maxValue);

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
}

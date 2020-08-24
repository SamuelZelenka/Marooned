using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterView : MonoBehaviour
{
    const float DOUBLECLICKTIME = 0.5f;

    static CharacterView focusedCharacterPortrait;
    private static CharacterView FocusedCharacterPortrait
    {
        get => focusedCharacterPortrait;
        set
        {
            if (FocusedCharacterPortrait != null)
            {
                focusedCharacterPortrait.ShowActive(false);
            }
            focusedCharacterPortrait = value;
            if (FocusedCharacterPortrait != null)
            {
                focusedCharacterPortrait.ShowActive(true);
            }
        }
    }

    public delegate void CharacterViewHandler(Character character);
    public event CharacterViewHandler OnSingleClick;
    public event CharacterViewHandler OnDoubleClick;

    [SerializeField] Image portrait = null;
    [SerializeField] Text characterName = null;

    [SerializeField] Text additionalText = null;

    [SerializeField] Text strength = null;
    [SerializeField] Text accuracy = null;
    [SerializeField] Text agility = null;
    [SerializeField] Text fortitude = null;
    [SerializeField] Text intelligence = null;
    [SerializeField] Text charisma = null;

    [SerializeField] Bar vitality = null;
    [SerializeField] Bar loyalty = null;
    [SerializeField] Bar energy = null;
    [SerializeField] Bar hunger = null;
    [SerializeField] Bar hygiene = null;

    [SerializeField] AbilityUI[] abilityIcons = new AbilityUI[4];

    [SerializeField] Text bountyText = null;
    [SerializeField] Bar bounty = null;

    [SerializeField] Image focusedBorder = null;

    [SerializeField] Text moves = null;

    [SerializeField] MouseHoverImage effectPrefab = null;
    [SerializeField] Transform effectParent = null;

    List<MouseHoverImage> activeEffects = new List<MouseHoverImage>();

    float latestClickTime = 0;

    Character character;

    [Header("Toggle Settings")]
    [SerializeField] bool showActiveEffects = false;
    [SerializeField] bool selectedByClick = true;

    public void SetCharacter(Character newCharacter)
    {
        if (character)
            UnSubscribe();
        character = newCharacter;
        if (character)
        {
            Subscribe();
            UpdateAllUI();
        }
        else
        {
            HideAllUI();
        }
    }
    public void SetAdditionalText(string text)
    {
        additionalText.text = text;
    }

    private void Subscribe()
    {
        character.OnUnitMoved += UpdateMoves;
        character.characterData.OnAnyResourceChanged += UpdateResourceBars;
        character.characterData.OnEffectChanged += UpdateEffectIcons;
    }
    private void UnSubscribe()
    {
        character.OnUnitMoved -= UpdateMoves;
        character.characterData.OnAnyResourceChanged -= UpdateResourceBars;
        character.characterData.OnEffectChanged -= UpdateEffectIcons;
    }

    private void UpdateAllUI()
    {
        this.gameObject.SetActive(true);
        CharacterData data = character.characterData;

        if (portrait)
            portrait.sprite = character.portrait;
        if (characterName)
            characterName.text = data.CharacterName;

        if (strength)
            strength.text = data.Strength.CurrentValue.ToString();
        if (accuracy)
            accuracy.text = data.Accuracy.CurrentValue.ToString();
        if (agility)
            agility.text = data.Agility.CurrentValue.ToString();
        if (fortitude)
            fortitude.text = data.Fortitude.CurrentValue.ToString();
        if (intelligence)
            intelligence.text = data.Intelligence.CurrentValue.ToString();
        if (charisma)
            charisma.text = data.Charisma.CurrentValue.ToString();

        if (bountyText)
            bountyText.text = data.BountyLevel.ToString();
        if (bounty)
            bounty.EnqueueChange(new Bar.ProgressStatus(data.BountyLevel.XP, data.BountyLevel.XPLevelUpRequirement, 0));

        for (int i = 0; i < abilityIcons.Length && i < character.Abilities.Count; i++)
        {
            if (abilityIcons[i])
            {
                abilityIcons[i].UpdateUI(character.Abilities[i]);
            }
        }

        UpdateResourceBars();
        UpdateEffectIcons();
    }

    private void HideAllUI() => this.gameObject.SetActive(false);

    private void UpdateResourceBars()
    {
        CharacterData data = character.characterData;
        if (vitality)
        {
            vitality.EnqueueChange(new Bar.ProgressStatus(data.Vitality.CurrentValue, data.Vitality.MaxValue, 0));
        }
        if (loyalty)
        {
            loyalty.EnqueueChange(new Bar.ProgressStatus(data.Loyalty.CurrentValue, data.Loyalty.MaxValue, 0));
        }
        if (energy)
        {
            energy.EnqueueChange(new Bar.ProgressStatus(data.Energy.CurrentValue, data.Energy.MaxValue, 0));
        }
        if (hunger)
        {
            hunger.EnqueueChange(new Bar.ProgressStatus(data.Hunger.CurrentValue, data.Hunger.MaxValue, 0));
        }
        if (hygiene)
        {
            hygiene.EnqueueChange(new Bar.ProgressStatus(data.Hygiene.CurrentValue, data.Hygiene.MaxValue, 0));
        }
    }
    private void UpdateEffectIcons()
    {
        if (showActiveEffects)
        {
            //Sync Active effects with GameObjects
            if (activeEffects.Count < character.characterData.activeEffects.Count) // too many
            {
                while (activeEffects.Count < character.characterData.activeEffects.Count)
                {
                    activeEffects.Add(Instantiate(effectPrefab, effectParent));
                }
            }
            if (activeEffects.Count > character.characterData.activeEffects.Count) // too few
            {
                while (activeEffects.Count > character.characterData.activeEffects.Count)
                {
                    Destroy(activeEffects[0].gameObject);
                    activeEffects.RemoveAt(0);
                }
            }

            for (int i = 0; i < character.characterData.activeEffects.Count; i++)
            {
                string effectDescription = character.characterData.activeEffects[i].GetDescription();
                Sprite effectSprite = character.characterData.activeEffects[i].EffectSprite;
                activeEffects[i].UpdateUI(effectDescription, effectSprite);
            }
        }
    }
    private void UpdateMoves()
    {
        if (moves)
            moves.text = $"Moves: {character.remainingMovementPoints}";
    }


    public void ShowActive(bool isActive) => focusedBorder.enabled = isActive;

    public void OnClick()
    {
        SingleClick();
        if (Time.time - latestClickTime < DOUBLECLICKTIME)
        {
            DoubleClick();
        }
        latestClickTime = Time.time;
    }
    private void SingleClick()
    {
        OnSingleClick?.Invoke(character);
        if (selectedByClick)
        {
            FocusedCharacterPortrait = this;
        }
    }
    private void DoubleClick()
    {
        OnDoubleClick?.Invoke(character);
    }
}



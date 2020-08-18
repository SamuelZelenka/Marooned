using UnityEngine;
using UnityEngine.UI;

public class CharacterDetailsView : MonoBehaviour
{
    [SerializeField] Image portrait = null;
    [SerializeField] Text characterName = null;

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

    [SerializeField] Text bounty = null;
    [SerializeField] Image bountyFill = null;

    [SerializeField] Text moves = null;

    public void UpdateValues(Character character)
    {
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

        if (vitality)
        {
            vitality.SetMaxValue(data.Vitality.MaxValue);
            vitality.SetCurrentValue(data.Vitality.CurrentValue);
        }
        if (loyalty)
        {
            loyalty.SetMaxValue(data.Loyalty.MaxValue);
            loyalty.SetCurrentValue(data.Loyalty.CurrentValue);
        }
        if (energy)
        {
            energy.SetMaxValue(data.Energy.MaxValue);
            energy.SetCurrentValue(data.Energy.CurrentValue);
        }
        if (hunger)
        {
            hunger.SetMaxValue(data.Hunger.MaxValue);
            hunger.SetCurrentValue(data.Hunger.CurrentValue);
        }
        if (hygiene)
        {
            hygiene.SetMaxValue(data.Hygiene.MaxValue);
            hygiene.SetCurrentValue(data.Hygiene.CurrentValue);
        }


        if (bounty)
            bounty.text = data.BountyLevel.ToString();
        if (bountyFill)
            bountyFill.fillAmount = (float)data.BountyLevel.XP / (float)data.BountyLevel.XPLevelUpRequirement;

        if (moves)
            moves.text = $"Moves: {character.remainingMovementPoints}";
    }
}

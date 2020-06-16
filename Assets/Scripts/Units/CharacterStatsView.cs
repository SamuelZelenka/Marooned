using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterStatsView : MonoBehaviour
{
    [SerializeField] Text characterName = null;
    [SerializeField] Text strength = null;
    [SerializeField] Text stamina = null;
    [SerializeField] Text constitution = null;
    [SerializeField] Text agility = null;
    [SerializeField] Text toughness = null;
    [SerializeField] Text accuracy = null;
    [SerializeField] Text bounty = null;
    [SerializeField] Slider vitality = null;
    [SerializeField] Slider energy = null;
    [SerializeField] Slider hunger = null;
    [SerializeField] Slider hygiene = null;
    [SerializeField] Text loyalty = null;

    Sprite Portrait;

    bool isDragging;
    private void Start()
    {
        characterName.text = "Debug Name";
        strength.text = $"Strength: {0}";
        stamina.text = $"Stamina: {0}";
        constitution.text = $"Constitution: {0}";
        agility.text = $"Agility: {0}";
        toughness.text = $"Toughness: {0}";
        accuracy.text = $"Accuracy: {0}";
        bounty.text = $"{0}";
        vitality.value = 5;
        energy.value = 5;
        hunger.value = 5;
        hygiene.value = 5;
        loyalty.text = $"{0}";
    }

    public void UpdateValues(Character character)
    {
        vitality.maxValue = character.characterData.MaxVitality;
        hygiene.maxValue = character.characterData.MaxHygiene;
        energy.maxValue = character.characterData.MaxEnergy;
        hunger.maxValue = character.characterData.MaxHunger;

        characterName.text = $"{character.characterData.characterName}";
        strength.text = $"Strength: {character.characterData.Strength}";
        stamina.text = $"Stamina: {character.characterData.Stamina}";
        constitution.text = $"Constitution: {character.characterData.Constitutuion}";
        agility.text = $"Agility: {character.characterData.Agility}";
        toughness.text = $"Toughness: {character.characterData.Toughness}";
        accuracy.text = $"Accuracy: {character.characterData.Accuracy}";
        bounty.text = $"{0}";
        vitality.value = 5;
        energy.value = 5;
        hunger.value = 5;
        hygiene.value = 5;
        loyalty.text = $"{0}";
        Portrait = character.characterData.portrait;
    }
}

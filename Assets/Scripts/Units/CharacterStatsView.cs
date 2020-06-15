using UnityEngine;
using UnityEngine.UI;

public class CharacterStatsView : MonoBehaviour
{
    [SerializeField] Text characterName;
    [SerializeField] Text strength;
    [SerializeField] Text stamina;
    [SerializeField] Text constitution;
    [SerializeField] Text agility;
    [SerializeField] Text toughness;
    [SerializeField] Text accuracy;
    [SerializeField] Text bounty;
    [SerializeField] Slider vitality;
    [SerializeField] Slider energy;
    [SerializeField] Slider hunger;
    [SerializeField] Slider hygiene;
    [SerializeField] Text loyalty;

    Sprite Portrait;
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

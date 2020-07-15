using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    bool isReady;
    [SerializeField] Image abilityImage = null;
    [SerializeField] Image disabledImage = null;
    [SerializeField] Text abilityCost = null;
    [SerializeField] Button abilityButton = null;
    [SerializeField] MouseHoverImage enabledHoverImage = null;


    public void UpdateUI(Ability ability)
    {

        if (HexGridController.ActiveCharacter.playerControlled)
        {
            if (ability.cost - 1 < HexGridController.ActiveCharacter.characterData.Energy.CurrentValue)
            {
                enabledHoverImage.UpdateUI(
                    $"{ability.abilityName}\n" +
                    $"{ability.abilityDescription} \n\n" +
                    $"<color=#00ff00> Ready </color> \n" +
                    $"{ability.cost} Energy",
                    ability.AbilitySprite);
                abilityImage.sprite = ability.AbilitySprite;
                Enabled(true);
            }
            else
            {
                enabledHoverImage.UpdateUI(
                    $"{ability.abilityName}\n" +
                    $"{ability.abilityDescription} \n\n" +
                    $"<color=red> Not enough energy </color> \n" +
                    $"{ability.cost} Energy",
                    ability.AbilitySprite);
                abilityCost.text = ability.cost.ToString();
                Enabled(false);
            }
        }
    }
    private void Enabled(bool isEnabled)
    {
        if (isEnabled)
        {
            disabledImage.gameObject.SetActive(false);
            abilityButton.interactable = true;
        }
        else
        {
            disabledImage.gameObject.SetActive(true);
            abilityButton.interactable = false;
        }
    }
}

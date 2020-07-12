using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    bool isReady;
    [SerializeField] Image abilityImage;
    [SerializeField] Image disabledImage;
    [SerializeField] Text abilityCost;
    [SerializeField] Button abilityButton;
    
    public void UpdateUI(Ability ability)
    {

        if (HexGridController.ActiveCharacter.playerControlled)
        {
            if (ability.cost - 1 < HexGridController.ActiveCharacter.characterData.Energy.CurrentValue)
            {
                abilityImage.sprite = ability.AbilitySprite;
                Enabled(true);
            }
            else
            {
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

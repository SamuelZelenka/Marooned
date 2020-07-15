using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUIController : MonoBehaviour
{
    [SerializeField] List<AbilityUI> abilityUIObjects = new List<AbilityUI>();
    [SerializeField] GameObject disableOverlay;
    [SerializeField] Text disableText;


    // Update is called once per frame
    public void UpdateUI()
    {
        List<bool> disabledAbilities = new List<bool>();
        Character activeCharacter = HexGridController.ActiveCharacter;

        if (DisableAbilities("Wait For your turn", !activeCharacter.playerControlled))
        {
            return;
        }
        if (DisableAbilities("Stunned", activeCharacter.IsStunned))
        {
            return;
        }

        for (int i = 0; i < activeCharacter.Abilities.Count; i++)
            {
                abilityUIObjects[i].UpdateUI(activeCharacter.Abilities[i]);
            }
            EnableAbilities();
    }
    public bool DisableAbilities(string reason, bool condition)
    {
        if (condition)
        {
            disableOverlay.gameObject.SetActive(true);
            disableText.text = reason;
            return true;
        }
        disableOverlay.gameObject.SetActive(false);
        return false;
    }
    public void EnableAbilities()
    {
        disableOverlay.gameObject.SetActive(false);
    }
}

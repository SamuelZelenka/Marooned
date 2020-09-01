using UnityEngine;
using UnityEngine.UI;

public class AbilityUIController : MonoBehaviour
{
    [SerializeField] GameObject disableOverlay = null;
    [SerializeField] Text disableText = null;
    private void OnEnable()
    {
        CombatSystem.OnAbilityUsed += UpdateUIOnAbility;
    }
    private void OnDisable()
    {
        CombatSystem.OnAbilityUsed -= UpdateUIOnAbility;
    }

    public void UpdateUIOnAbility() => UpdateUI(HexGridController.ActiveCharacter);

    public void UpdateUI( Character activeCharacter)
    {
        if (DisableAbilities("Wait For your turn", !activeCharacter.playerControlled))
        {
            return;
        }
        if (DisableAbilities("Downed", activeCharacter.isDowned))
        {
            return;
        }
        if (DisableAbilities("Stunned", activeCharacter.IsStunned))
        {
            return;
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

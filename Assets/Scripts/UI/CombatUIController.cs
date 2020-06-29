using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUIController : MonoBehaviour
{
    public PartyDisplay crew = null;
    public SelectedCharacterDisplay selectedCharacter = null;

    public void UpdateCrewDisplay()
    {
        crew.UpdateParty();
    }
    public void SelectCharacter()
    {
        selectedCharacter.UpdateUI();
    }
}

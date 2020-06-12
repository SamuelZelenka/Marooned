using System.Collections.Generic;
using UnityEngine;
public class CharacterAbilities : MonoBehaviour
{
    Ability selectedAbility = null;
    List<Ability> abilities = null;
    Character myCharacter = null;
    public void SelectAbility(int abilityIndex, Character user)
    {
        selectedAbility = abilities[abilityIndex];
        List<HexCell> validCells = selectedAbility.targetType.GetValidCells(myCharacter.myGrid.GetCell());
        myCharacter = user;
    }
}
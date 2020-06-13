using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CharacterAbilities
{
    Ability selectedAbility = null;
    public List<Ability> abilities = null;
    public void SelectAbility(int abilityIndex, Character user)
    {
        selectedAbility = abilities[abilityIndex];
        List<HexCell> validCells = selectedAbility.targetType.GetValidCells(user.myGrid.GetCell());
    }
}
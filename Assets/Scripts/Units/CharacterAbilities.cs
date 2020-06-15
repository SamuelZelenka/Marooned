using System.Collections.Generic;
[System.Serializable]
public class CharacterAbilities
{
    public Dictionary<string, Ability> abilityDictionary = new Dictionary<string, Ability>() 
    {
        {"debug", new DebugAdjacentAbility() } 
    };

    Ability selectedAbility = null;
    public List<Ability> abilities = null;
    public void SelectAbility(int abilityIndex, Character user)
    {
        selectedAbility = abilities[abilityIndex];
        List<HexCell> validCells = selectedAbility.targetType.GetValidCells(user.myGrid.GetCell());
    }
}
using System.Collections.Generic;
[System.Serializable]
public class CharacterAbilities
{
    public Dictionary<string, Ability> abilityDictionary = new Dictionary<string, Ability>() 
    {
        {"debug", new DebugAdjacentAbility() } 
    };

    public CharacterAbilities(Character owner)
    {
        this.owner = owner;
        abilities = new List<Ability>();
    }

    Character owner;
    List<Ability> abilities;

    public Ability SelectAbility(int abilityIndex, out List<HexCell> possibleTargets)
    {
        possibleTargets = abilities[abilityIndex].targetType.GetValidCells(owner.myGrid.GetCell());
        return abilities[abilityIndex];
    }
}
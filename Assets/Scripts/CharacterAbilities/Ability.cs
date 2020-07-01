using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    public static Dictionary<int, Ability> abilityDictionary = new Dictionary<int, Ability>()
    {
        {0, new ChainWhip()},
        {100, new Slice()}
    };

    public string abilityName;
    public string abilityDescription;
    public Sprite abilitySprite;

    public int cost;
    public bool RequireSkillCheck
    {
        protected set;
        get;
    }
    public CharacterStatType AttackerSkillcheck
    {
        protected set;
        get;
    }
    public CharacterStatType TargetSkillcheck
    {
        protected set;
        get;
    }

    protected List<Effect> effects = new List<Effect>();
    public TargetType targeting;

    public void Use(Character target, SkillcheckSystem.CombatOutcome outcome)
    {
        if (outcome == SkillcheckSystem.CombatOutcome.Miss)
        {
            return;
        }
        foreach (var item in effects)
        {
            item.ApplyEffect(target, outcome);
        }
    }
}

public abstract class TargetType
{
    public abstract List<HexCell> GetValidTargets(HexCell fromCell);
    public abstract List<HexCell> GetAffectedCells(HexCell fromCell, HexCell targetCell);
    public List<Character> GetAffectedCharacters(HexCell fromCell, HexCell targetCell)
    {
        List<Character> affectedCharacters = new List<Character>();
        foreach (var cell in GetAffectedCells(fromCell, targetCell))
        {
            Character character = cell.Unit as Character;
            if (character)
            {
                affectedCharacters.Add(character);
            }
        }
        return affectedCharacters;
    }
}

public class SingleTargetAdjacent : TargetType
{
    public override List<HexCell> GetValidTargets(HexCell fromCell)
    {
        return CellFinder.GetAllAdjacent(fromCell);
    }
    public override List<HexCell> GetAffectedCells(HexCell fromCell, HexCell targetCell)
    {
        List<HexCell> affectedCells = new List<HexCell>();
        affectedCells.Add(targetCell);
        return affectedCells;
    }
}

public class SwipeAdjacent : TargetType
{
    public override List<HexCell> GetValidTargets(HexCell fromCell)
    {
        return CellFinder.GetAllAdjacent(fromCell);
    }
    public override List<HexCell> GetAffectedCells(HexCell fromCell, HexCell targetCell)
    {
        List<HexCell> affectedCells = new List<HexCell>();
        affectedCells.Add(targetCell);

        //Sides
        HexDirection dirToSelected = HexDirectionExtension.GetDirectionTo(fromCell, targetCell);
        HexCell previousCell = fromCell.GetNeighbor(dirToSelected.Previous(), true, false, false, false);
        if (previousCell)
        {
            affectedCells.Add(previousCell);
        }
        HexCell nextCell = fromCell.GetNeighbor(dirToSelected.Next(), true, false, false, false);
        if (nextCell)
        {
            affectedCells.Add(nextCell);
        }
        return affectedCells;
    }
}
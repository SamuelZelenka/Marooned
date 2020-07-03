using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    public static Dictionary<int, Ability> abilityDictionary = new Dictionary<int, Ability>()
    {
        {0, new ChainWhip(0)},
        {1, new GrabAndPull(1)},
        {100, new Slice(100)}
    };

    public string abilityName;
    public string abilityDescription;
    public Sprite AbilitySprite
    {
        private set;
        get;
    }

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

    const string path = "AbilitySprites/";
    protected Ability(int abilityIndex)
    {
        abilityName = ToString();
        AbilitySprite = Resources.Load<Sprite>(path + "AbilityIcon" + abilityIndex);
    }

    public void Use(Character attacker, Character target, SkillcheckSystem.CombatOutcome outcome)
    {
        if (outcome == SkillcheckSystem.CombatOutcome.Miss)
        {
            return;
        }
        foreach (var item in effects)
        {
            item.ApplyEffect(attacker, target, outcome);
        }
    }
    public string CreateCombatLogMessage(Character attacker, List<Character> targets)
    {
        string targetsToString = "";

        // Billy, John, Robert and Andrew
        if (targets.Count > 0)
        {
            if (targets.Count > 1)
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    if (i == targets.Count - 1)
                    {
                        targetsToString += $"and {targets[i].characterData.characterName}.";
                    }
                    else if (i == targets.Count - 2)
                    {
                        targetsToString += $"{targets[i].characterData.characterName} ";
                    }
                    else
                    {
                        targetsToString += $"{targets[i].characterData.characterName}, ";
                    }
                }
            }
            else
            {
                targetsToString = $"{targets[0].characterData.characterName}.";
            }
        }
        else
        {
            return $"{attacker.characterData.characterName} used {abilityName} but completely failed at aiming.\n";
        }
        return $"{attacker.characterData.characterName} used {abilityName} on {targetsToString}\n";
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
        HexDirection dirToSelected = HexDirectionExtension.GetDirectionToNeighbor(fromCell, targetCell);
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

public class AnySingleTarget : TargetType
{
    public override List<HexCell> GetValidTargets(HexCell fromCell)
    {
        return CellFinder.GetAllCells(fromCell.myGrid, true, true);
    }

    public override List<HexCell> GetAffectedCells(HexCell fromCell, HexCell targetCell)
    {
        List<HexCell> affectedCells = new List<HexCell>();
        affectedCells.Add(targetCell);
        return affectedCells;
    }
}

public class SingleTargetRanged : TargetType
{
    public override List<HexCell> GetValidTargets(HexCell fromCell)
    {
        return CellFinder.GetAllCells(fromCell.myGrid, true, true);
    }

    public override List<HexCell> GetAffectedCells(HexCell fromCell, HexCell targetCell)
    {
        List<HexCell> affectedCells = new List<HexCell>();
        affectedCells.Add(targetCell);
        return affectedCells;
    }
}
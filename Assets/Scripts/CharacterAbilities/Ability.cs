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
    protected List<Effect> effects = new List<Effect>();
    public TargetType targeting;

    public void Use(Character target)
    {
        foreach (var item in effects)
        {
            item.ApplyEffect(target);
        }
    }
}

public abstract class TargetType
{
    public abstract List<HexCell> GetValidTargets(HexCell fromCell);
    public abstract List<HexCell> GetAffectedCells(HexCell fromCell, HexCell selectedCell);
}

public class SingleTargetAdjacent : TargetType
{
    public override List<HexCell> GetValidTargets(HexCell fromCell)
    {
        return CellFinder.GetAllAdjacent(fromCell);
    }
    public override List<HexCell> GetAffectedCells(HexCell fromCell, HexCell selectedCell)
    {
        List<HexCell> affectedCells = new List<HexCell>();
        affectedCells.Add(selectedCell);
        return affectedCells;
    }
}

public class SwipeAdjacent : TargetType
{
    public override List<HexCell> GetValidTargets(HexCell fromCell)
    {
        return CellFinder.GetAllAdjacent(fromCell);
    }
    public override List<HexCell> GetAffectedCells(HexCell fromCell, HexCell selectedCell)
    {
        List<HexCell> affectedCells = new List<HexCell>();
        affectedCells.Add(selectedCell);

        //Sides
        HexDirection dirToSelected = HexDirectionExtension.GetDirectionTo(fromCell, selectedCell);
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
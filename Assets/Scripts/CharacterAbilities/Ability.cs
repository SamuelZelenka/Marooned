using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    public static Dictionary<int, Ability> abilityDictionary = new Dictionary<int, Ability>()
    {
        {0, new ChainWhip()}
    };

    public string abilityName;
    public string abilityDescription;
    public Sprite abilitySprite;

    protected int cost;
    protected List<Effect> effects = new List<Effect>();
    public TargetType targetType;

    //public virtual void Select(Character character)
    //{

    //}

    public virtual void Use(Character character)
    {
        character.characterData.Energy.CurrentValue -= cost;
    }

    //public virtual void DeSelect()
    //{

    //}
}

public abstract class TargetType
{
    public abstract List<HexCell> GetValidCells(HexCell fromCell);
    public abstract List<HexCell> GetAffectedCells(HexCell fromCell, HexCell selectedCell);
}

public class SingleTargetAdjacent : TargetType
{
    public override List<HexCell> GetValidCells(HexCell fromCell)
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
    public override List<HexCell> GetValidCells(HexCell fromCell)
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
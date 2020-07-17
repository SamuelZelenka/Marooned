using System.Collections.Generic;
using UnityEngine;

public class ActiveHexObject : HexObject
{
    public enum EffectType { Healing}
    [SerializeField] EffectType effectType;
    int changePerTurn;
    Character endOfTurnConnectedCharacter;

    public void SetupObject(int changePerTurn, Character connectedCharacter)
    {
        this.changePerTurn = changePerTurn;
        endOfTurnConnectedCharacter = connectedCharacter;
        Subscribe(true);
    }

    
    void Subscribe(bool status)
    {
        if (status)
        {
            switch (effectType)
            {
                case EffectType.Healing:
                    CombatTurnSystem.OnTurnEnding += HealPlayerControlledCharacters;
                    break;
            }
        }
        else
        {
            switch (effectType)
            {
                case EffectType.Healing:
                    CombatTurnSystem.OnTurnEnding -= HealPlayerControlledCharacters;
                    break;
            }
        }
    }

    public override void Despawn()
    {
        Subscribe(false);
        base.Despawn();
    }

    void HealPlayerControlledCharacters(Character character)
    {
        if (character != endOfTurnConnectedCharacter)
        {
            return;
        }
        List<HexCell> cells = new List<HexCell>();
        cells.PopulateListWithMatchingConditions(Location.Neighbors, (c) => c.Traversable == true, (c) => c.Unit != null);

        foreach (HexCell item in cells)
        {
            Character foundCharacter = item.Unit as Character;
            if (foundCharacter != null && foundCharacter.playerControlled)
            {
                foundCharacter.characterData.Vitality.CurrentValue += changePerTurn;
            }
        }
    }
}

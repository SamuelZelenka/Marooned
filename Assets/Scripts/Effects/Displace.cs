using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Displace : Effect
{
    bool pull;
    int hexes;

    public Displace(bool pull, int hexes)
    {
        this.pull = pull;
        this.hexes = hexes;
    }


    public override void ApplyEffect(Character attacker, Character target, SkillcheckSystem.CombatOutcome outcome)
    {
        HexDirection directionToTarget = HexDirectionExtension.GetDirectionTo(attacker.Location, target.Location);

        HexCell newCell = target.Location;
        for (int i = 0; i < hexes; i++)
        {
            HexCell cellToTry = pull ? newCell.GetNeighbor(HexDirectionExtension.Opposite(directionToTarget)) : newCell.GetNeighbor(directionToTarget);
            if (cellToTry.Unit != null)
            {
                break;
            }
            newCell = cellToTry;
        }
        target.Location = newCell;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : HexUnit
{
    public CharacterData characterData = new CharacterData();

    public bool isStunned;
    public CharacterAbilities abilities;

    public delegate void EffectHandler(Effect effect);
    public event EffectHandler OnEffectApplied;

    HexCell savedShipLocation;
    public HexCell SavedShipLocation
    {
        get => savedShipLocation;
        set
        {
            if (savedShipLocation)
            {
                savedShipLocation.Unit = null;
            }
            savedShipLocation = value;
            value.Unit = this;
        }
    }

    public void AddEffect(Effect effect)
    {
        characterData.activeEffects.Add(effect);
        effect.ApplyEffect(this);
        Debug.Log(characterData.activeEffects.Count);
    }

    public void RemoveEffects(Effect effect)
    {
        if (characterData.activeEffects.Contains(effect))
        {
            characterData.activeEffects.Remove(effect);
            characterData.removedEffects.Add(effect);
        }
        else
        {
            Debug.LogError($"ActiveEffects does not contain this effect");
        }
    }

    public void EffectTickUpdate()
    {
        if (characterData.activeEffects.Count > 0)
        {
            for (int i = 0; i < characterData.activeEffects.Count; i++)
            {
                characterData.activeEffects[i].EffectTick(this);
                OnEffectApplied?.Invoke(characterData.activeEffects[i]);
            }
        }
    }

    //TEMP METHOD!!!!!
    public void ButtonAddEffect(string effect)
    {
        switch (effect)
        {
            case "stun":
                AddEffect(new Stun(2));
                break;
            case "bleed":
                AddEffect(new Bleed(2));
                break;
            default:
                break;
        }

    }
    //TEMP METHOD!!!!!

    public override bool CanMoveTo(HexCell cell)
    {
        if (cell.Unit)
        {
            return false;
        }
        return true;
    }

    public override void StartNewTurn()
    {
        remainingMovementPoints = defaultMovementPoints;
    }

    HexCell target;
    public override IEnumerator PerformAutomaticTurn()
    {
        //Do turn
        CombatTurnSystem.OnTurnBegining?.Invoke(this);

        if (target)
        {
            yield return MoveToTarget();
        }
        else
        {
            target = FindTarget();
            Debug.Log("AI Finding path from " + Location.coordinates.ToString() + " to " + target.coordinates.ToString());
            yield return MoveToTarget();
        }

        //End turn
        CombatTurnSystem.OnTurnEnding?.Invoke(this);
        yield return null;
    }

    IEnumerator MoveToTarget()
    {
        Pathfinding.FindPath(Location, target, this, playerControlled);
        int tries = 0;
        while (!Pathfinding.HasPath && tries < 100) //Target unreachable
        {
            HexCell adjacentToTarget = target.GetNeighbor(HexDirectionExtension.ReturnRandomDirection());
            if (adjacentToTarget)
            {
                Pathfinding.FindPath(Location, adjacentToTarget, this, playerControlled);
            }
            tries++;
        }
        if (Pathfinding.HasPath)
        {
            yield return Travel(Pathfinding.GetReachablePath(this, out int cost));
            remainingMovementPoints -= cost;
            Pathfinding.ClearPath();
        }
        if (Location == target)
        {
            target = null;
        }
    }

    private HexCell FindTarget()
    {
        HexCell newTarget = myGrid.GetRandomFreeCell();
        while (Location == newTarget)
        {
            newTarget = myGrid.GetRandomFreeCell();
        }
        return newTarget;
    }
}

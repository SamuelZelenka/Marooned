using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : HexUnit
{
    public string characterName;

    public bool isStunned;
    public CharacterResources resources;
    public CharacterStats stats;

    public delegate void EffectHandler(Effect effect);
    public event EffectHandler OnEffectApplied;

    public List<Effect> activeEffects = new List<Effect>();
    List<Effect> removedEffects = new List<Effect>();

    public Sprite portrait;
    public CrewSimulation.ShipJob ShipJob { get; set; } = CrewSimulation.ShipJob.None;

    public void AddEffect(Effect effect)
    {
        activeEffects.Add(effect);
        effect.ApplyEffect(this);
        Debug.Log(activeEffects.Count);
    }

    public void RemoveEffects(Effect effect)
    {
        if (activeEffects.Contains(effect))
        {
            activeEffects.Remove(effect);
            removedEffects.Add(effect);
        }
        else
        {
            Debug.LogError($"ActiveEffects does not contain this effect");
        }
    }

    public void EffectTickUpdate()
    {
        if (activeEffects.Count > 0)
        {
            for (int i = 0; i < activeEffects.Count; i++)
            {
                activeEffects[i].EffectTick(this);
                OnEffectApplied?.Invoke(activeEffects[i]);
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

    public override IEnumerator PerformAutomaticTurn()
    {
        throw new System.NotImplementedException();
    }
}

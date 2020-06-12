using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillpowerDecrease : Effect
{
    int amount;
    public WillpowerDecrease(int amount)
    {
        description = $"Reduces Willpower by {amount}";
        this.amount = amount;
    }
    public override void ApplyEffect(Character character) { }
    public override void EffectTick(Character character)
    {
        character.resources.Loyalty -= amount;
    }
    public override string GetData()
    {
        return "";
    }
}

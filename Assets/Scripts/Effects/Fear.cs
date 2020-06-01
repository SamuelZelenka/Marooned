using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fear : Effect
{
    int damage;
    public Fear(int damage)
    {
        description = "";
        this.damage = damage;
    }
    public override void ApplyEffect(Character character) { }
    public override void EffectTick(Character character)
    {
        character.resources.Vitality -= damage;
    }
}

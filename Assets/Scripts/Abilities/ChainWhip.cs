using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainWhip : Ability
{
    public ChainWhip()
    {
        targetType = new SingleTargetAdjacent();
        effects.Add(new Bleed(5));
    }
}

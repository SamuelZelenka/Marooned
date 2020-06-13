using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCharacter : Character
{
    private void Start()
    {
        characterName = "DebugCharacter";
        abilities.abilities.Add(new DebugAdjacentAbility());
    }
}

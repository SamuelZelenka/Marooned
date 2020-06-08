using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected string abilityName;
    protected string abilityDescription;

    protected int cost;
    protected List<Effect> effects;
}

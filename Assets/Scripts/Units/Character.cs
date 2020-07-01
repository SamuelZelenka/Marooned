using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : HexUnit
{
    public CharacterData characterData = new CharacterData();

    public GameObject animatedArrow;

    public bool isStunned;
    public List<Ability> Abilities { get; set; } = new List<Ability>();
    public List<int> abilityID = new List<int>();

    public delegate void EffectHandler(Effect effect);
    public event EffectHandler OnEffectApplied;

    //Used for storing locations on the player ship
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

    private void Awake()
    {
        //Setup abilities
        foreach (var item in abilityID)
        {
            if (Ability.abilityDictionary.TryGetValue(item, out Ability foundAbility))
            {
                Abilities.Add(foundAbility);
            }
            else
            {
                Debug.LogError("Ability ID " + item + " not found in dictionary");
            }
        }
    }

    public override void ShowUnitActive(bool status)
    {
        base.ShowUnitActive(status);
        animatedArrow.SetActive(status);
    }

    #region Effects and abilities
    public void AddEffect(TickEffect effect)
    {
        characterData.activeEffects.Add(effect);
        Debug.Log(characterData.activeEffects.Count);
    }

    public void RemoveEffects(TickEffect effect)
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

    public Ability SelectAbility(int abilityIndex, out List<HexCell> possibleTargets)
    {
        possibleTargets = Abilities[abilityIndex].targeting.GetValidTargets(Location);
        return Abilities[abilityIndex];
    }

    public Ability SelectAbility(Ability ability, out List<HexCell> possibleTargets)
    {
        if (!Abilities.Contains(ability))
        {
            throw new System.ArgumentException("Selected ability not part of characters abilities. In " + characterData.characterName + " - " + ability.ToString());
        }
        possibleTargets = ability.targeting.GetValidTargets(Location);
        return ability;
    }
    #endregion


    public override bool CanEnter(HexCell cell)
    {
        if (cell.Unit)
        {
            return false;
        }
        return true;
    }

    public override void StartNewTurn()
    {
        base.StartNewTurn();
        remainingMovementPoints = defaultMovementPoints;
    }
    public void SelectThisCharacter()
    {
        HexGridController.SelectedCell = this.Location;
    }

    #region AI
    public void SetAI(AI ai) => aiController = ai;
    public void SetNextAction(ActionGroup actionGroup) => nextAction = actionGroup;
    AI aiController;
    ActionGroup nextAction;
    //HexCell target;
    public override IEnumerator PerformAutomaticTurn()
    {
        //Do turn
        CombatTurnSystem.OnTurnBegining?.Invoke(this);

        yield return aiController.CalculateAvailableActions(this);
        if (nextAction != null)
        {
            if (nextAction.cellToEndTurnOn != Location)
            {
                yield return MoveToTargetCell(nextAction.cellToEndTurnOn);
            }
            if (nextAction.abilityToUse != null)
            {
                CombatSystem.instance.SelectAbility(nextAction.abilityToUse);
                CombatSystem.instance.UseAbility(nextAction.cellAbilityTarget);
            }
        }

        nextAction = null;

        //End turn
        CombatTurnSystem.OnTurnEnding?.Invoke(this);
        yield return null;
    }

    IEnumerator MoveToTargetCell(HexCell targetCell)
    {
        Pathfinding.FindPath(Location, targetCell, this, playerControlled);
        List<HexCell> path = Pathfinding.GetWholePath();
        yield return Travel(path);
        remainingMovementPoints -= path.Count;
        Pathfinding.ClearPath();
    }
    #endregion
}

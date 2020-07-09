using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : HexUnit
{
    public CharacterData characterData = new CharacterData();

    public GameObject animatedArrow;
    public CharacterOverHeadUI overHeadUI;

    public bool isStunned;
    public List<Character> tauntedBy = new List<Character>();

    public List<Ability> Abilities { get; set; } = new List<Ability>();
    public List<int> abilityID = new List<int>();

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

    public void CombatSetup() => characterData.ObjectInitialized();

    public override void ShowUnitActive(bool status)
    {
        base.ShowUnitActive(status);
        animatedArrow.SetActive(status);
    }

    #region Effects and abilities
    private void EffectTickUpdate()
    {
        if (characterData.activeEffects.Count > 0)
        {
            for (int i = 0; i < characterData.activeEffects.Count; i++)
            {
                characterData.activeEffects[i].EffectTick(this);
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

    public bool isFriendlyTo(Character otherCharacter)
    {
        //If berserk : return false;
        //if othercharacter is berserk : return false;
        return playerControlled == otherCharacter.playerControlled;
    }

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
        if (!isStunned)
        {
            remainingMovementPoints = defaultMovementPoints;
            characterData.Energy.CurrentValue += CharacterData.DEFAULTENERGYREGEN;
        }
        else
        {
            remainingMovementPoints = 0;
        }
        base.StartNewTurn();
    }

    public void TurnEnded()
    {
        EffectTickUpdate();
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

        if (!isStunned)
        {
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
                else
                {
                    CombatSystem.instance.EndActiveCharacterTurn();
                }
            }

            nextAction = null;
        }
        else
        {
            CombatSystem.instance.EndActiveCharacterTurn();
        }
    }

    IEnumerator MoveToTargetCell(HexCell targetCell)
    {
        Pathfinding.FindPath(Location, targetCell, this, playerControlled);
        List<HexCell> path = Pathfinding.GetWholePath();
        yield return Travel(path);
        Pathfinding.ClearPath();
    }
    #endregion
}

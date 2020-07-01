using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    //Singleton included in Setup below

    [Header("References")]
    public HexGrid hexGrid;
    public Transform playerCharacterParent;
    public Transform enemyCharacterParent;

    public GameObject combatCanvas;
    public GameObject mapView;
    public GameObject combatView;
    public CombatTurnSystem turnSystem;
    public CombatUIController uiController;
    [SerializeField] SkillcheckSystem skillcheckSystem = null;

    Player humanPlayer;
    Ship playerShip;

    [Header("Setup")]
    [HideInInspector]
    public BattleMap managementMap;
    public BattleMap[] battleMaps;
    public List<Character> debugEnemies = new List<Character>();

    public delegate void CombatHandler();
    public static CombatHandler OnCombatStart;
    public static CombatHandler OnCombatEnd;
    public static CombatHandler OnAbilityUsed;

    private Ability selectedAbility;
    List<HexCell> validTargetHexes;
    private List<HexCell> ValidTargetHexes
    {
        get => validTargetHexes;
        set
        {
            if (validTargetHexes != null)
            {
                foreach (var item in validTargetHexes)
                {
                    item.ShowHighlight(false, HexCell.HighlightType.ValidCombatInteraction);
                }
            }
            validTargetHexes = value;
            if (validTargetHexes != null)
            {
                foreach (var item in validTargetHexes)
                {
                    item.ShowHighlight(true, HexCell.HighlightType.ValidCombatInteraction);
                }
            }
        }
    }
    List<HexCell> abilityAffectedHexes;
    private List<HexCell> AbilityAffectedHexes
    {
        get => abilityAffectedHexes;
        set
        {
            if (abilityAffectedHexes != null)
            {
                foreach (var item in abilityAffectedHexes)
                {
                    item.ShowHighlight(false, HexCell.HighlightType.AbilityAffected);
                }
            }
            abilityAffectedHexes = value;
            if (abilityAffectedHexes != null)
            {
                foreach (var item in abilityAffectedHexes)
                {
                    item.ShowHighlight(true, HexCell.HighlightType.AbilityAffected);
                }
            }
        }
    }
    List<Character> abilityTargetCharacters = new List<Character>();

    #region Setup References

    #region Singleton
    public static CombatSystem instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Another instance of : " + instance.ToString() + " was tried to be instanced, but was destroyed from gameobject: " + this.transform.name);
            GameObject.Destroy(this);
            return;
        }
        instance = this;
        SessionSetup.OnHumanPlayerCreated += DoSetup;
    }
    #endregion

    private void DoSetup(Player humanPlayer)
    {
        this.humanPlayer = humanPlayer;
        playerShip = humanPlayer.Ship;
        //Unsubscribe
        SessionSetup.OnHumanPlayerCreated -= DoSetup;
    }
    #endregion

    private void OnEnable()
    {
        CombatTurnSystem.OnTurnBegining += ResetSelections;
        HexCell.OnHexCellHoover += MarkCellsAndCharactersToBeAffected;
        HexUnit.OnUnitMoved += ResetHexes;
    }

    private void OnDisable()
    {
        CombatTurnSystem.OnTurnBegining -= ResetSelections;
        HexCell.OnHexCellHoover -= MarkCellsAndCharactersToBeAffected;
        HexUnit.OnUnitMoved -= ResetHexes;
    }

    public void StartCombat()
    {
        OnCombatStart?.Invoke();
        OpenCombatCanvas(true);
        SetUpCombat(0);
    }

    private void SetUpCombat(int size)
    {
        hexGrid.Load(battleMaps[size], false);
        List<Character> allCharacters = new List<Character>();

        //Player characters
        allCharacters.AddRange(humanPlayer.Crew);
        uiController.UpdateCrewDisplay(humanPlayer.Crew);


        //Enemy characters
        AI aiController = new AI(debugEnemies, humanPlayer.Crew);
        foreach (Character charactersToSpawn in debugEnemies)
        {
            //Instantiate enemies
            Character spawnedCharacter = Instantiate(charactersToSpawn);
            spawnedCharacter.transform.SetParent(enemyCharacterParent);

            spawnedCharacter.myGrid = hexGrid;
            spawnedCharacter.SetAI(aiController);

            //Add character to grid
            hexGrid.AddUnit(spawnedCharacter, hexGrid.GetFreeCellForCharacterSpawn(HexCell.SpawnType.AnyEnemy), false);

            //Add character to list of all characters involved in combat
            allCharacters.Add(spawnedCharacter);
        }

        turnSystem.SetupNewCombat(allCharacters);
        turnSystem.StartCombat();
    }

    public void EndCombat()
    {
        OnCombatEnd?.Invoke();
        hexGrid.Load(managementMap, false);
        HexGridController.CurrentMode = HexGridController.GridMode.Map;

        foreach (var item in humanPlayer.Crew)
        {
            //Add move characters back to their saved location
            item.Location = hexGrid.GetCell(item.SavedShipLocation.coordinates);
            item.SavedShipLocation = item.Location;
        }
        OpenCombatCanvas(false);
    }

    private void ResetHexes(HexUnit unitMoved)
    {
        ValidTargetHexes = null;
        AbilityAffectedHexes = null;
        selectedAbility = null;
    }

    private void ResetSelections(Character activeCharacter)
    {
        selectedAbility = null;
        ValidTargetHexes = new List<HexCell>();
        abilityTargetCharacters = new List<Character>();
    }

    private void MarkCellsAndCharactersToBeAffected(HexCell targetCell)
    {
        if (selectedAbility != null && HexGridController.ActiveCharacter != null && ValidTargetHexes.Contains(targetCell))
        {
            AbilityAffectedHexes = selectedAbility.targeting.GetAffectedCells(HexGridController.ActiveCharacter.Location, targetCell);
            abilityTargetCharacters = selectedAbility.targeting.GetAffectedCharacters(HexGridController.ActiveCharacter.Location, targetCell);
        }
    }

    //Called from the UI when a player selects an ability
    public void SelectAbility(int selection)
    {
        selectedAbility = HexGridController.ActiveCharacter.SelectAbility(selection, out List<HexCell> abilityTargetHexes);
        Debug.Log("Selected ability " + selectedAbility.abilityName);
        ValidTargetHexes = abilityTargetHexes;
    }

    //Used by the AI when called from the character
    public void SelectAbility(Ability selection)
    {
        selectedAbility = HexGridController.ActiveCharacter.SelectAbility(selection, out List<HexCell> abilityTargetHexes);
        Debug.Log("Selected ability " + selectedAbility.abilityName);
        ValidTargetHexes = abilityTargetHexes;
    }

    public void UseAbility(HexCell selectedCellForTarget)
    {
        MarkCellsAndCharactersToBeAffected(selectedCellForTarget);
        if (selectedAbility != null && ValidTargetHexes != null && AbilityAffectedHexes != null && ValidTargetHexes.Contains(selectedCellForTarget)) //Have selected an ability
        {
            if (HexGridController.ActiveCharacter.characterData.Energy.CurrentValue >= selectedAbility.cost) //Have enough energy
            {
                Debug.Log("Using ability " + selectedAbility.abilityName);
                if (selectedAbility.RequireSkillCheck)
                {
                    skillcheckSystem.OnCombatOutcomesDecided += ResolveUsedAbility;

                    Character attacker = HexGridController.ActiveCharacter;
                    List<Character> targets = selectedAbility.targeting.GetAffectedCharacters(attacker.Location, selectedCellForTarget);

                    skillcheckSystem.StartContestedSkillcheck(attacker, targets, selectedAbility.AttackerSkillcheck, selectedAbility.TargetSkillcheck);
                }
                else
                {
                    ResolveUsedAbility();
                }
            }
            else
            {
                Debug.Log("Not enough energy");
            }
        }
        else
        {
            Debug.Log("Action not possible. No action selected or clicked hex is not a valid hex");
        }
    }

    //Required outcomes
    private void ResolveUsedAbility(List<SkillcheckSystem.CombatOutcome> attackOutcome)
    {
        skillcheckSystem.OnCombatOutcomesDecided -= ResolveUsedAbility;
        for (int i = 0; i < abilityTargetCharacters.Count; i++)
        {
            selectedAbility.Use(abilityTargetCharacters[i], attackOutcome[i]);
        }
        HexGridController.ActiveCharacter.characterData.Energy.CurrentValue -= selectedAbility.cost;
        OnAbilityUsed?.Invoke();
    }

    //No required outcomes
    private void ResolveUsedAbility()
    {
        for (int i = 0; i < abilityTargetCharacters.Count; i++)
        {
            selectedAbility.Use(abilityTargetCharacters[i], SkillcheckSystem.CombatOutcome.NormalHit);
        }
        HexGridController.ActiveCharacter.characterData.Energy.CurrentValue -= selectedAbility.cost;
        OnAbilityUsed?.Invoke();
    }

    private void OpenCombatCanvas(bool showCombat)
    {
        combatCanvas.SetActive(showCombat);
    }
}

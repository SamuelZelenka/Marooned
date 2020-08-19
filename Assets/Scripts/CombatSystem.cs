using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    //Singleton included in Setup below

    [Header("References")]
    [SerializeField] HexGrid hexGrid = null;
    [SerializeField] Transform enemyCharacterParent = null;

    [SerializeField] CombatTurnSystem turnSystem = null;
    [SerializeField] CombatUIView combatUIView = null;
    [SerializeField] HexGridController hexGridController = null;


    [Header("Setup")]
    [HideInInspector]
    public BattleMap managementMap;
    public BattleMap[] battleMaps;
    public List<Character> debugEnemies = new List<Character>();

    public delegate void CombatHandler();
    public static CombatHandler OnCombatStart;
    public static CombatHandler OnCombatEnd;

    private Character latestCharacter;

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
    List<Character> hostileAbilityAffectedCharacters = new List<Character>();
    List<Character> friendlyAbilityAffectedCharacters = new List<Character>();
    List<Character> AllAffectedCharacters
    {
        get
        {
            List<Character> allCharacters = new List<Character>();
            allCharacters.AddRange(hostileAbilityAffectedCharacters);
            allCharacters.AddRange(friendlyAbilityAffectedCharacters);
            return allCharacters;
        }
    }

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
    }
    #endregion


    private void OnEnable()
    {
        CombatTurnSystem.OnTurnBeginning += NewCharacterTurn;
        HexCell.OnHexCellHoover += MarkCellsAndCharactersToBeAffected;
        HexGridController.OnCellSelected += UseAbility;
    }

    private void OnDisable()
    {
        CombatTurnSystem.OnTurnBeginning -= NewCharacterTurn;
        HexCell.OnHexCellHoover -= MarkCellsAndCharactersToBeAffected;
        HexGridController.OnCellSelected -= UseAbility;
    }
    #if UNITY_EDITOR
    private void Update()
    {
        CheatInput();
    }
    #endif
    public void StartCombat(Player defender, Player attacker)
    {
        OnCombatStart?.Invoke();
        hexGridController.StartCombatMode();
        SetUpCombat(0, defender, attacker);
    }

    private void SetUpCombat(int size, Player defender, Player attacker)
    {
        hexGrid.Load(battleMaps[size], false);
        List<Character> allCharacters = new List<Character>();

        bool playerIsDefending = defender.IsHuman;

        if (playerIsDefending)
        {
            //Player characters
            allCharacters.AddRange(defender.Crew);
            //Enemy controlled characters
            AI aiController = new AI(attacker.Crew, defender.Crew);
            foreach (Character charactersToSpawn in attacker.Crew)
            {
                //Instantiate enemies
                Character spawnedCharacter = Instantiate(charactersToSpawn);
                spawnedCharacter.transform.SetParent(enemyCharacterParent);

                spawnedCharacter.myGrid = hexGrid;
                spawnedCharacter.SetAI(aiController);

                //Add character to grid
                HexCell validCell = hexGrid.Cells.ReturnRandomElementWithCondition((c) => c.IsFree == true, (c) => c.TypeOfSpawnPos == HexCell.SpawnType.AnyEnemy);
                hexGrid.AddUnit(spawnedCharacter, validCell, false);

                //Add character to list of all characters involved in combat
                allCharacters.Add(spawnedCharacter);
            }
        }
        else
        {
            //Player characters
            allCharacters.AddRange(attacker.Crew);
            //Enemy controlled characters
            AI aiController = new AI(defender.Crew, attacker.Crew);
            foreach (Character charactersToSpawn in defender.Crew)
            {
                //Instantiate enemies
                Character spawnedCharacter = Instantiate(charactersToSpawn);
                spawnedCharacter.transform.SetParent(enemyCharacterParent);

                spawnedCharacter.myGrid = hexGrid;
                spawnedCharacter.SetAI(aiController);

                //Add character to grid
                HexCell validCell = hexGrid.Cells.ReturnRandomElementWithCondition((c) => c.IsFree == true, (c) => c.TypeOfSpawnPos == HexCell.SpawnType.AnyEnemy);
                hexGrid.AddUnit(spawnedCharacter, validCell, false);

                //Add character to list of all characters involved in combat
                allCharacters.Add(spawnedCharacter);
            }
        }

        foreach (Character character in allCharacters)
        {
            character.CombatSetup();
        }
        if (playerIsDefending) combatUIView.UpdateCrew(defender.Crew);
        else combatUIView.UpdateCrew(defender.Crew);

        turnSystem.SetupNewCombat(allCharacters);
        turnSystem.StartCombat();
    }

    public void IsCombatOver()
    {
        bool isCombatOver = true;

        // Combat end conditions

        foreach (Character character in HexGridController.player.Crew)
        {
            isCombatOver = character.IsCharacterDown() ? true : false ;
        }
        if (isCombatOver) EndCombat();
    }

    public void EndCombat()
    {
        OnCombatEnd?.Invoke();
        hexGrid.Load(managementMap, false);
        HexGridController.CurrentMode = HexGridController.GridMode.Map;

        foreach (var item in HexGridController.player.Crew)
        {
            //Add move characters back to their saved location
            item.Location = hexGrid.GetCell(item.SavedShipLocation.coordinates);
            item.SavedShipLocation = item.Location;
        }
        Debug.Log("Combat End");
    }

    private void NewCharacterTurn(Character newTurnCharacter)
    {
        if (latestCharacter != null)
        {
            newTurnCharacter.OnUnitMoved -= ResetSelections;
        }
        latestCharacter = newTurnCharacter;
        ResetSelections();
        newTurnCharacter.OnUnitMoved += ResetSelections;
    }

    private void ResetSelections()
    {
        ValidTargetHexes = null;
        AbilityAffectedHexes = null;
        selectedAbility = null;
    }

    private void MarkCellsAndCharactersToBeAffected(HexCell targetCell)
    {
        Character activeCharacter = HexGridController.ActiveCharacter;
        if (selectedAbility != null && activeCharacter != null && ValidTargetHexes.Contains(targetCell))
        {
            AbilityAffectedHexes = selectedAbility.targeting.GetAffectedCells(activeCharacter.Location, targetCell);
            selectedAbility.targeting.GetAffectedCharacters(activeCharacter, activeCharacter.Location, targetCell, out hostileAbilityAffectedCharacters, out friendlyAbilityAffectedCharacters);

            foreach (var character in hostileAbilityAffectedCharacters)
            {
                float hitchance = GetHitChance(activeCharacter, character, selectedAbility.HostileHitChanceSkillcheck);
                string hitChanceString = Utility.FactorToPercentageText(hitchance);
                character.overHeadUI.SetAdditionalText(hitChanceString);
            }
            foreach (var character in friendlyAbilityAffectedCharacters)
            {
                float hitchance = GetHitChance(activeCharacter, character, selectedAbility.FriendlyHitChanceSkillcheck);
                string hitChanceString = Utility.FactorToPercentageText(hitchance);
                character.overHeadUI.SetAdditionalText(hitChanceString);
            }
        }
    }

    //Called from the UI when a player selects an ability
    public void SelectAbility(int selection)
    {
        if (HexGridController.ActiveCharacter.IsStunned)
        {
            return;
        }
        ResetSelections();
        selectedAbility = HexGridController.ActiveCharacter.SelectAbility(selection, out List<HexCell> abilityTargetHexes);
        Debug.Log("Selected ability " + selectedAbility.abilityName);
        ValidTargetHexes = abilityTargetHexes;
    }

    //Used by the AI when called from the character
    public void SelectAbility(Ability selection)
    {
        ResetSelections();
        selectedAbility = HexGridController.ActiveCharacter.SelectAbility(selection, out List<HexCell> abilityTargetHexes);
        Debug.Log("Selected ability " + selectedAbility.abilityName);
        ValidTargetHexes = abilityTargetHexes;
    }

    public void UseAbility(HexCell selectedCellForTarget)
    {
        Character abilityUser = HexGridController.ActiveCharacter;

        if (abilityUser == null)
            return;

        MarkCellsAndCharactersToBeAffected(selectedCellForTarget);
        if (selectedAbility != null && ValidTargetHexes != null && AbilityAffectedHexes != null && ValidTargetHexes.Contains(selectedCellForTarget)) //Have selected an ability and the target cell is a valid target
        {
            if (abilityUser.characterData.Energy.CurrentValue >= selectedAbility.cost) //Have enough energy
            {
                Debug.Log("Using ability " + selectedAbility.abilityName);


                List<Character> charactersHit = new List<Character>();
                List<Character> charactersCrit = new List<Character>();

                foreach (var character in friendlyAbilityAffectedCharacters)
                {
                    if (Random.value < GetHitChance(abilityUser, character, selectedAbility.FriendlyHitChanceSkillcheck))
                    {
                        charactersHit.Add(character);
                    }
                }

                foreach (var character in hostileAbilityAffectedCharacters)
                {
                    if (Random.value < GetHitChance(abilityUser, character, selectedAbility.HostileHitChanceSkillcheck))
                    {
                        charactersHit.Add(character);
                    }
                }


                selectedAbility.Use(abilityUser, charactersHit, charactersCrit, AbilityAffectedHexes);
                PostAbilityCalculations();
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

    private void PostAbilityCalculations()
    {
        HexGridController.ActiveCharacter.characterData.Energy.CurrentValue -= selectedAbility.cost;
        HexGridController.ActiveCharacter.logMessage.AddLine(selectedAbility.CreateCombatLogMessage(HexGridController.ActiveCharacter, AllAffectedCharacters));
        HexGridController.ActiveCharacter.logMessage.SetAbilitySprite(selectedAbility.AbilitySprite);
        EndActiveCharacterTurn();
    }

    public void EndActiveCharacterTurn() => turnSystem.EndActiveCharacterTurn();

    private float GetHitChance(Character attacker, Character defender, CharacterStatType attackerStatType)
    {
        if (attackerStatType == CharacterStatType.NONE)
        {
            return 1f;
        }
        float hitChance = 0.5f;

        hitChance += attacker.characterData.GetStat(attackerStatType).CurrentValue * 0.1f;
        hitChance -= defender.characterData.Agility.CurrentValue * 0.1f;
        return hitChance;
    }

#region Cheats

    void CheatInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && Input.GetKey(KeyCode.LeftShift))
        {
            HexGridController.ActiveCharacter.characterData.Energy.CurrentValue = HexGridController.ActiveCharacter.characterData.Energy.MaxValue;
        }
        if (Input.GetKeyDown(KeyCode.M) && Input.GetKey(KeyCode.LeftShift))
        {
            HexGridController.ActiveCharacter.remainingMovementPoints = HexGridController.ActiveCharacter.defaultMovementPoints;
        }
    }
#endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [Header("References")]
    public HexGrid hexGrid;
    public Transform playerCharacterParent;
    public Transform enemyCharacterParent;

    public GameObject combatCanvas;
    public GameObject mapView;
    public GameObject combatView;
    public CombatTurnSystem turnSystem;

    Player humanPlayer;
    Ship playerShip;

    [Header("Setup")]
    [HideInInspector]
    public BattleMap managementMap;
    public BattleMap[] battleMaps;
    public Character[] debugEnemies;

    public delegate void CombatHandler();
    public static CombatHandler OnCombatStart;
    public static CombatHandler OnCombatEnd;


    public Character ActiveCharacter
    {
        private set;
        get;
    }

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

    #region Setup References
    private void Awake() => SessionSetup.OnHumanPlayerCreated += DoSetup;

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
        CombatTurnSystem.OnTurnBegining += SetActiveCharacter;
        HexCell.OnHexCellHoover += MouseOverHexCell;
        HexUnit.OnUnitMoved += ResetHexes;
    }

    private void OnDisable()
    {
        CombatTurnSystem.OnTurnBegining -= SetActiveCharacter;
        HexCell.OnHexCellHoover -= MouseOverHexCell;
        HexUnit.OnUnitMoved -= ResetHexes;
    }

    private void SetActiveCharacter(Character activeCharacter)
    {
        Debug.Log(activeCharacter.name);
        selectedAbility = null;
        ValidTargetHexes = new List<HexCell>();
        ActiveCharacter = activeCharacter;
    }

    public void SelectAbility(int selection)
    {
        selectedAbility = ActiveCharacter.SelectAbility(selection, out List<HexCell> abilityTargetHexes);
        Debug.Log("Selected ability " + selectedAbility.abilityDescription);
        ValidTargetHexes = abilityTargetHexes;
    }

    private void MouseOverHexCell(HexCell mouseOverCell)
    {
        if (selectedAbility != null && ActiveCharacter != null && ValidTargetHexes.Contains(mouseOverCell))
        {
            AbilityAffectedHexes = selectedAbility.targetType.GetAffectedCells(ActiveCharacter.Location, mouseOverCell);
        }
    }

    private void ResetHexes(HexUnit unitMoved)
    {
        ValidTargetHexes = null;
        AbilityAffectedHexes = null;
        selectedAbility = null;
    }

    public void UseAbility(HexCell cellClickedOn)
    {
        MouseOverHexCell(cellClickedOn);
        if (selectedAbility != null && AbilityAffectedHexes != null && ValidTargetHexes.Contains(cellClickedOn))
        {
            Debug.Log("Using ability " + selectedAbility.abilityDescription);
            foreach (var item in AbilityAffectedHexes)
            {
                if (item.Unit is Character)
                {
                    selectedAbility.Use(item.Unit as Character);
                }
            }
        }
    }

    public void StartCombat()
    {
        OnCombatStart?.Invoke();
        ChangeView(true);
        SetUpCombat(0);
    }

    private void SetUpCombat(int size)
    {
        hexGrid.Load(battleMaps[size], false);
        List<Character> allCharacters = new List<Character>();

        //Player characters
        allCharacters.AddRange(humanPlayer.Crew);

        //Enemy characters
        foreach (Character charactersToSpawn in debugEnemies)
        {
            //Instantiate enemies
            Character spawnedCharacter = Instantiate(charactersToSpawn);
            spawnedCharacter.transform.SetParent(enemyCharacterParent);

            spawnedCharacter.myGrid = hexGrid;

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

        foreach (var item in humanPlayer.Crew)
        {
            //Add move characters back to their saved location
            item.Location = hexGrid.GetCell(item.SavedShipLocation.coordinates);
            item.SavedShipLocation = item.Location;
        }
        ChangeView(false);
    }

    private void ChangeView(bool showCombat)
    {
        combatCanvas.SetActive(showCombat);
        combatView.SetActive(showCombat);
        mapView.SetActive(!showCombat);
    }
}

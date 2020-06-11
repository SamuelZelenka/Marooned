using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    #region Singleton
    public static TurnSystem instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Another instance of : " + instance.ToString() + " was tried to be instanced, but was destroyed from gameobject: " + this.transform.name);
            GameObject.Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    List<Player> mapTurnOrder = new List<Player>();
    int activeMapPlayerIndex = 0;

    List<Character> battleTurnOrder = new List<Character>();
    int activeBattleTurnOrderIndex = 0;

    public void AddPlayerToTurnOrder(Player player)
    {
        mapTurnOrder.Add(player);
    }

    public void DoFirstTurn()
    {
        DoTurn(mapTurnOrder[0]);
    }

    public void EndTurn()
    {
        activeMapPlayerIndex++;
        if (activeMapPlayerIndex >= mapTurnOrder.Count)
        {
            activeMapPlayerIndex = 0;
        }
        DoTurn(mapTurnOrder[activeMapPlayerIndex]);
    }

    private void DoTurn(Player activePlayer)
    {
        Debug.Log("Starting new turn");
        activePlayer.StartNewTurn();

        if (activePlayer.IsHuman)
        {
            Debug.Log("Human player turn");
            return;
        }
        else
        {
            StartCoroutine(activePlayer.PerformAutomaticTurn());
        }
    }

}

public class Player
{
    List<Character> crew = new List<Character>();
    Ship ship;
    public bool IsHuman { get; private set; }
    CrewSimulation crewSimulation;

    /// <summary>
    /// Creates a human controlled player
    /// </summary>
    /// <param name="ship"></param>
    /// <param name="humanControlled"></param>
    /// <param name="crewSimulation"></param>
    public Player(Ship ship, bool humanControlled, CrewSimulation crewSimulation)
    {
        this.ship = ship;
        IsHuman = humanControlled;
        this.crewSimulation = crewSimulation;
    }

    /// <summary>
    /// Creates an AI controlled player
    /// </summary>
    /// <param name="ship"></param>
    /// <param name="humanControlled"></param>
    public Player(Ship ship, bool humanControlled = false)
    {
        this.ship = ship;
        IsHuman = humanControlled;
        this.crewSimulation = null;
        if (IsHuman)
        {
            Debug.LogWarning("Human player is set up with missing ship job system");
        }
    }

    public void StartNewTurn()
    {
        foreach (var item in crew)
        {
            item.remainingMovementPoints = item.defaultMovementPoints;
        }

        if (IsHuman)
        {
            OpenJobPanel();
        }
        else
        {
            ship.remainingMovementPoints = ship.defaultMovementPoints;
        }
    }

    private void OpenJobPanel()
    {
        if (crewSimulation)
        {
            crewSimulation.OpenJobPanel();
        }
        else
        {
            Debug.LogWarning("Missing ship job system");
        }
    }

    public void RunCrewSimulation()
    {
        crewSimulation.RunSimulation();
    }

    private void StartMapMovement()
    {

    }

    public IEnumerator PerformAutomaticTurn()
    {
        yield return ship.PerformAutomaticTurn();
        TurnSystem.instance.EndTurn();
    }
}

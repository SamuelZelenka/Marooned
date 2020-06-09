using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridController : MonoBehaviour
{
    #region Singleton
    public static HexGridController instance;
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

    List<Player> turnOrder = new List<Player>();
    Player activePlayer;
    int indexActivePlayer = 0;

    public void AddPlayerToTurnOrder(Player player)
    {
        turnOrder.Add(player);
    }

    public void EndTurn()
    {
        indexActivePlayer++;
        if (indexActivePlayer >= turnOrder.Count)
        {
            indexActivePlayer = 0;
        }
        activePlayer = turnOrder[indexActivePlayer];
        DoTurn();
    }

    private void DoTurn()
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
    List<HexUnit> units = new List<HexUnit>();
    public bool IsHuman { get; private set; }
    CrewSimulation crewSimulation;

    public Player(List<HexUnit> unitsToControl, bool humanControlled, CrewSimulation crewSimulation)
    {
        units = unitsToControl;
        IsHuman = humanControlled;
        this.crewSimulation = crewSimulation;
    }

    public Player(List<HexUnit> unitsToControl, bool humanControlled = false)
    {
        units = unitsToControl;
        IsHuman = humanControlled;
        this.crewSimulation = null;
        if (IsHuman)
        {
            Debug.LogWarning("Human player is set up with missing ship job system");
        }
    }

    public void StartNewTurn()
    {
        foreach (var item in units)
        {
            item.remainingMovementPoints = item.defaultMovementPoints;
        }

        if (IsHuman)
        {
            OpenJobPanel();
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
        foreach (var item in units)
        {
            yield return item.PerformAutomaticTurn();
        }
        HexGridController.instance.EndTurn();
    }
}

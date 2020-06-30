using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public List<Character> Crew { get; private set; }
    public Ship Ship { get; private set; }
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
        Crew = new List<Character>();
        this.Ship = ship;
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
        Crew = new List<Character>();
        this.Ship = ship;
        IsHuman = humanControlled;
        this.crewSimulation = null;
        if (IsHuman)
        {
            Debug.LogWarning("Human player is set up with missing ship job system");
        }
    }

    public void StartNewTurn()
    {
        Ship.StartNewTurn();
        HexGridController.ActiveShip = Ship;

        if (IsHuman)
        {
            OpenJobPanel();
        }
    }

    private void OpenJobPanel()
    {
        if (crewSimulation)
        {
            crewSimulation.NewTurnSimulation();
        }
        else
        {
            Debug.LogWarning("Missing ship job system");
        }
    }

    public void RunCrewSimulation()
    {
        crewSimulation.RunJobSimulation();
    }

    private void StartMapMovement()
    {
        
    }

    public IEnumerator PerformAutomaticTurn()
    {
        yield return Ship.PerformAutomaticTurn();
        MapTurnSystem.instance.EndTurn();
    }
}

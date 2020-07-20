using System.Collections;
using System.Collections.Generic;

public class Player
{
    public List<Character> Crew { get; private set; }
    public Ship Ship { get; private set; }
    public bool IsHuman { get; private set; }
    CrewSimulation crewSimulation;
    public PlayerData PlayerData { get; private set; }

    /// <summary>
    /// Creates a human controlled player
    /// </summary>
    /// <param name="ship"></param>
    /// <param name="humanControlled"></param>
    /// <param name="crewSimulation"></param>
    public Player(Ship ship, bool humanControlled, CrewSimulation crewSimulation)
    {
        PlayerData = new PlayerData();
        Crew = new List<Character>();
        this.Ship = ship;
        ship.Setup(PlayerData.ShipData);
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
    }

    public void StartNewTurn()
    {
        HexGridController.ActiveShip = Ship;

        if (IsHuman)
        {
            StartManagementMode();
        }
        else
        {
            Ship.MakeUnitActive();
        }
    }

    private void StartManagementMode()
    {
        HexGridController.CurrentMode = HexGridController.GridMode.Management;
        crewSimulation.NewTurnSimulation();
    }

    public IEnumerator PerformAutomaticTurn()
    {
        yield return Ship.PerformAutomaticTurn();
        MapTurnSystem.instance.EndTurn();
    }
}

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

    public Player(List<HexUnit> unitsToControl, bool humanControlled)
    {
        units = unitsToControl;
        IsHuman = humanControlled;
    }

    public void StartNewTurn()
    {
        foreach (var item in units)
        {
            item.movement = item.maxMovement;
        }
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

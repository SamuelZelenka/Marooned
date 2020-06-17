using System.Collections.Generic;
using UnityEngine;

public class MapTurnSystem : MonoBehaviour
{
    #region Singleton
    public static MapTurnSystem instance;
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
     
    public void AddPlayerToTurnOrder(Player player) => mapTurnOrder.Add(player);

    public void DoFirstTurn() => DoTurn(mapTurnOrder[0]);

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
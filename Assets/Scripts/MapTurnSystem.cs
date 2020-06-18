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

    List<Player> turnOrder = new List<Player>();
    int activeIndex = 0;
     
    public void AddPlayerToTurnOrder(Player player) => turnOrder.Add(player);

    public void DoFirstTurn() => DoTurn(turnOrder[0]);

    public void EndTurn()
    {
        activeIndex++;
        if (activeIndex >= turnOrder.Count)
        {
            activeIndex = 0;
        }
        DoTurn(turnOrder[activeIndex]);
    }

    private void DoTurn(Player activePlayer)
    {
        Debug.Log("Starting new turn");
        activePlayer.StartNewTurn();

        if (!activePlayer.IsHuman)
        {
            StartCoroutine(activePlayer.PerformAutomaticTurn());
        }
    }
}
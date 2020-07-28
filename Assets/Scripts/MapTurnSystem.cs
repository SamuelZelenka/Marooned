using System.Collections.Generic;
using UnityEngine;

public class MapTurnSystem : MonoBehaviour
{
    [SerializeField] WorldController worldController = null;

    int day = 0;
    public int Day
    {
        get => day;
        set
        {
            day = value;
            OnDayEnded?.Invoke();
            if (value == WEEKLENGTH)
            {
                day = 1;
                Week++;
            }
        }
    }
    int week;
    int Week
    {
        get => week;
        set
        {
            week = value;
            OnWeekEnded?.Invoke();
        }
    }
    const int WEEKLENGTH = 7;
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

    public delegate void MapTurnHandler();
    public MapTurnHandler OnDayEnded;
    public MapTurnHandler OnWeekEnded;
    public MapTurnHandler OnFirstTurnStarted;


    List<Player> turnOrder = new List<Player>();
    int activeIndex = 0;

    public void AddPlayerToTurnOrder(Player player) => turnOrder.Add(player);
    public void AddPlayerToFirstPositionInTurnOrder(Player player) => turnOrder.Insert(0, player);

    public void DoFirstTurn()
    {
        OnFirstTurnStarted?.Invoke();
        DoTurn(turnOrder[0]);
        day = 1;
        week = 1;
    }

    public void EndTurn()
    {
        activeIndex++;
        if (activeIndex >= turnOrder.Count)
        {
            activeIndex = 0;
            Day++;
        }
        DoTurn(turnOrder[activeIndex]);
    }

    private void DoTurn(Player activePlayer)
    {
        Debug.Log("Starting new turn");
        activePlayer.StartNewTurn();

        if (!activePlayer.IsHuman)
        {
            int visionRange = worldController.GetVisionRange(HexGridController.player.PlayerData);
            StartCoroutine(activePlayer.PerformAutomaticTurn(visionRange));
        }
    }
}
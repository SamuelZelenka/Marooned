using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardingController : MonoBehaviour
{
    [SerializeField] ResourceView[] resources = null;
    [SerializeField] ShipInspectView inspectView = null;
    ShipData boardedShip = null;

    public void Setup(ShipData boardedShip)
    {
        this.boardedShip = boardedShip;
    }

    void UpdateUI()
    {
        inspectView.Setup(boardedShip, true);
    }

    void StealResource(ResourceType resourceType, int numberOfItemsToSteal)
    {
        PlayerData playerData = HexGridController.player.PlayerData;
        boardedShip.GetResource(resourceType).Value -= numberOfItemsToSteal; //Take from boarded ship
        playerData.ShipData.GetResource(resourceType).Value += numberOfItemsToSteal; //Give to player
        UpdateUI();
    }

    //Button call
    public void StealResource(int resourceIndex) => StealResource((ResourceType)resourceIndex, Mathf.RoundToInt(resources[resourceIndex].GetSliderValue()));

    //Button call
    public void StealAllResources()
    {
        for (int i = 0; i < (int)ResourceType.MAX; i++)
        {
            ResourceType resourceType = (ResourceType)i;
            int numbers = boardedShip.GetResource(resourceType).Value;
            StealResource(resourceType, numbers);
        }
    }
}

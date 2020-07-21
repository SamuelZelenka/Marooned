using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MerchantController : MonoBehaviour
{
    [SerializeField] ResourceView[] resources = null;
    [SerializeField] Text sellAllButtonText = null;
    public Harbor myHarbor;

    public void Setup()
    {
        for (int i = 0; i < resources.Length; i++)
        {
            UpdateUI((ResourceType)i);
        }
        SetSellAllButtontext();
    }

    void UpdateUI(ResourceType resourceType)
    {
        ShipData playerShipData = HexGridController.player.PlayerData.ShipData;
        resources[(int)resourceType].Setup(playerShipData.GetResource(resourceType), true, myHarbor.GetResourceValue(resourceType));
        SetSellAllButtontext();
    }

    void SellResource(ResourceType resourceType, int numbersOfItemsToSell)
    {
        PlayerData playerData = HexGridController.player.PlayerData;
        playerData.ShipData.GetResource(resourceType).Value -= numbersOfItemsToSell; //Remove from player
        playerData.Gold += numbersOfItemsToSell * myHarbor.GetResourceValue(resourceType);
        Debug.Log(playerData.Gold.ToString());
        UpdateUI(resourceType);
    }

    //Button call
    public void SellResource(int resourceIndex) => SellResource((ResourceType)resourceIndex, Mathf.RoundToInt(resources[resourceIndex].GetSliderValue()));

    //Button call
    public void SellAllResources()
    {
        ShipData playerShipData = HexGridController.player.PlayerData.ShipData;
        for (int i = 0; i < (int)ResourceType.MAX; i++)
        {
            ResourceType resourceType = (ResourceType)i;
            int numbers = playerShipData.GetResource(resourceType).Value;
            SellResource(resourceType, numbers);
        }
    }

    void SetSellAllButtontext()
    {
        ShipData playerShipData = HexGridController.player.PlayerData.ShipData;
        int totalValue = 0;
        for (int i = 0; i < (int)ResourceType.MAX; i++)
        {
            totalValue += playerShipData.GetResource((ResourceType)i).Value * myHarbor.GetResourceValue((ResourceType)i);
        }
        sellAllButtonText.text = $"Sell All (£{totalValue.ToString()})";
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantController : MonoBehaviour
{
    [SerializeField] ResourceView[] resources = null;
    public Harbor myHarbor;

    public void Setup()
    {
        for (int i = 0; i < resources.Length; i++)
        {
            UpdateUI((ResourceType)i);
        }
    }

    void UpdateUI(ResourceType resourceType)
    {
        ShipData playerShipData = HexGridController.player.PlayerData.ShipData;
        resources[(int)resourceType].Setup(playerShipData.GetResource(resourceType), true);
    }

    void SellResource(ResourceType resourceType)
    {
        PlayerData playerData = HexGridController.player.PlayerData;

        //Sell resource
        int selectedNumber = Mathf.RoundToInt(resources[(int)resourceType].GetSliderValue());
        playerData.ShipData.GetResource(resourceType).value -= selectedNumber; //Remove from player
        playerData.Gold += selectedNumber * myHarbor.GetResourceValue(resourceType);
        Debug.Log(playerData.Gold.ToString());
        UpdateUI(resourceType);
    }

    //Button call
    public void SellResource(int resourceIndex) => SellResource((ResourceType)resourceIndex);

    //Button call
    public void SellAllResources()
    {
        for (int i = 0; i < (int)ResourceType.MAX; i++)
        {
            SellResource(i);
        }
    }
}
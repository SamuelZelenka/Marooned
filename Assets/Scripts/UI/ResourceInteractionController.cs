using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceInteractionController : MonoBehaviour
{
    public enum Mode { Merchant, Boarding }
    [SerializeField] Mode mode;
    [SerializeField] ResourceView[] resources = null;
    [SerializeField] GameObject[] interactButtonObjects = null;
    [SerializeField] Text interactButtonText = null;
    Harbor myHarbor;
    ShipData activeShipData = null;

    private void OnEnable()
    {
        ResourceView.OnSliderValueChanged += SetInteractButtonText;
    }

    private void OnDisable()
    {
        ResourceView.OnSliderValueChanged -= SetInteractButtonText;
    }

    /// <summary>
    /// Used to set up merchants
    /// </summary>
    /// <param name="myHarbor"></param>
    public void Setup(Harbor myHarbor)
    {
        this.activeShipData = HexGridController.player.PlayerData.ShipData;
        this.myHarbor = myHarbor;
        for (int i = 0; i < resources.Length; i++)
        {
            UpdateUI((ResourceType)i, true);
        }
        SetInteractButtonText();
    }

    /// <summary>
    /// Used to setup for boarding or inspecting other ships
    /// </summary>
    /// <param name="inspectedShip"></param>
    /// <param name="boarding"></param>
    public void Setup(ShipData inspectedShip, bool boarding)
    {
        foreach (var item in interactButtonObjects)
        {
            item.SetActive(boarding);
        }

        this.activeShipData = inspectedShip;
        for (int i = 0; i < resources.Length; i++)
        {
            UpdateUI((ResourceType)i, boarding);
        }
        SetInteractButtonText();
    }

    void UpdateUI(ResourceType resourceType, bool interactable)
    {
        switch (mode)
        {
            case Mode.Merchant:
                resources[(int)resourceType].Setup(activeShipData.GetResource(resourceType), interactable, myHarbor.GetResourceValue(resourceType));
                break;
            case Mode.Boarding:
                resources[(int)resourceType].Setup(activeShipData.GetResource(resourceType), interactable, ShipData.GetDefaultValue(resourceType));
                break;
        }
        SetInteractButtonText();
    }

    //Button call
    public void SetAllSlidersToMax()
    {
        foreach (var item in resources)
        {
            item.ChangeSliderToMax();
        }
    }

    void SetInteractButtonText()
    {
        int totalValue = 0;
        for (int i = 0; i < resources.Length; i++)
        {
            ResourceType resourceType = (ResourceType)i;

            int valueOfItem = 0;

            switch (mode)
            {
                case Mode.Merchant:
                    valueOfItem = myHarbor.GetResourceValue(resourceType);
                    break;
                case Mode.Boarding:
                    valueOfItem = ShipData.GetDefaultValue(resourceType);
                    break;
            }

            totalValue += Mathf.RoundToInt(resources[i].GetSliderValue()) * valueOfItem;
        }

        switch (mode)
        {
            case Mode.Merchant:
                interactButtonText.text = $"Sell Marked Items (£{totalValue.ToString()})";
                break;
            case Mode.Boarding:
                interactButtonText.text = $"Steal Marked Items (£{totalValue.ToString()})";
                break;
        }
    }

    #region Merchant
    void SellResource(ResourceType resourceType, int numbersOfItemsToSell)
    {
        PlayerData playerData = HexGridController.player.PlayerData;
        playerData.ShipData.GetResource(resourceType).Value -= numbersOfItemsToSell; //Remove from player
        playerData.Gold += numbersOfItemsToSell * myHarbor.GetResourceValue(resourceType);
        UpdateUI(resourceType, true);
    }

    //Button call
    public void SellMarkedResources()
    {
        for (int i = 0; i < resources.Length; i++)
        {
            ResourceType resourceType = (ResourceType)i;
            int numberOfItems = Mathf.RoundToInt(resources[i].GetSliderValue());
            SellResource(resourceType, numberOfItems);
        }
    }
    #endregion

    #region Piracy
    void StealResource(ResourceType resourceType, int numberOfItemsToSteal)
    {
        PlayerData playerData = HexGridController.player.PlayerData;
        activeShipData.GetResource(resourceType).Value -= numberOfItemsToSteal; //Take from boarded ship
        playerData.ShipData.GetResource(resourceType).Value += numberOfItemsToSteal; //Give to player
        UpdateUI(resourceType, true);
    }

    //Button call
    public void StealMarkedResources()
    {
        for (int i = 0; i < resources.Length; i++)
        {
            ResourceType resourceType = (ResourceType)i;
            int numberOfItems = Mathf.RoundToInt(resources[i].GetSliderValue());
            StealResource(resourceType, numberOfItems);
        }
    }
    #endregion
}
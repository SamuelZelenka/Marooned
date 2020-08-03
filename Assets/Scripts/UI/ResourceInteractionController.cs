using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceInteractionController : MonoBehaviour
{
    public enum Mode { Merchant, Boarding }
    [SerializeField] Mode mode = Mode.Merchant;
    [SerializeField] InteractableResourceView[] resourceViewers = null;
    [SerializeField] GameObject[] interactButtonObjects = null;
    [SerializeField] Text interactButtonText = null;
    [SerializeField] Text totalValueText = null;
    Harbor myHarbor;
    ResourceInventory activeShipInventory = null;

    private void OnEnable()
    {
        InteractableResourceView.OnSliderValueChanged += SetTexts;
    }

    private void OnDisable()
    {
        InteractableResourceView.OnSliderValueChanged -= SetTexts;
    }

    /// <summary>
    /// Used to set up merchants
    /// </summary>
    /// <param name="myHarbor"></param>
    public void Setup(Harbor myHarbor)
    {
        this.activeShipInventory = HexGridController.player.PlayerData.Resources;
        this.myHarbor = myHarbor;
        foreach (InteractableResourceView viewer in resourceViewers)
        {
            UpdateUI(viewer.ResourceType, true);
        }
        SetTexts();
    }

    /// <summary>
    /// Used to setup for boarding or inspecting other ships
    /// </summary>
    /// <param name="inspectedShipInventory"></param>
    /// <param name="boarding"></param>
    public void Setup(ResourceInventory inspectedShipInventory, bool buttons, bool sliders)
    {
        foreach (var button in interactButtonObjects)
        {
            button.SetActive(buttons);
        }

        this.activeShipInventory = inspectedShipInventory;
        foreach (var item in resourceViewers)
        {
            UpdateUI(item.ResourceType, !buttons && !sliders);
        }
        SetTexts();
    }

    void UpdateUI(ResourceType resourceType, bool interactable)
    {
        switch (mode)
        {
            case Mode.Merchant:
                resourceViewers[(int)resourceType].Setup(activeShipInventory.GetResource(resourceType), interactable, myHarbor.GetResourceValue(resourceType));
                break;
            case Mode.Boarding:
                resourceViewers[(int)resourceType].Setup(activeShipInventory.GetResource(resourceType), interactable, ResourceInventory.GetDefaultValue(resourceType));
                break;
        }
        SetTexts();
    }

    //Button call
    public void SetAllSlidersToMax()
    {
        foreach (var item in resourceViewers)
        {
            item.ChangeSliderToMax();
        }
    }

    void SetTexts()
    {
        int totalValue = 0;
        int totalSelectedValue = 0;
        for (int i = 0; i < resourceViewers.Length; i++)
        {
            ResourceType resourceType = (ResourceType)i;

            int valueOfItem = 0;

            switch (mode)
            {
                case Mode.Merchant:
                    valueOfItem = myHarbor.GetResourceValue(resourceType);
                    break;
                case Mode.Boarding:
                    valueOfItem = ResourceInventory.GetDefaultValue(resourceType);
                    break;
            }

            totalValue += activeShipInventory.GetResource(resourceType).Value * valueOfItem;
            totalSelectedValue += Mathf.RoundToInt(resourceViewers[i].GetSliderValue()) * valueOfItem;
        }

        switch (mode)
        {
            case Mode.Merchant:
                interactButtonText.text = $"Sell Marked Items (£{totalSelectedValue.ToString()})";
                break;
            case Mode.Boarding:
                interactButtonText.text = $"Steal All Items (£{totalValue.ToString()})";
                break;
        }
        totalValueText.text = $"Total Value: {totalValue}";
    }


    #region Merchant
    void SellResource(ResourceType resourceType, int numbersOfItemsToSell)
    {
        PlayerData playerData = HexGridController.player.PlayerData;
        playerData.Resources.GetResource(resourceType).Value -= numbersOfItemsToSell; //Remove from player
        playerData.Gold += numbersOfItemsToSell * myHarbor.GetResourceValue(resourceType);
        UpdateUI(resourceType, true);
    }

    //Button call
    public void SellMarkedResources()
    {
        for (int i = 0; i < resourceViewers.Length; i++)
        {
            ResourceType resourceType = (ResourceType)i;
            int numberOfItems = Mathf.RoundToInt(resourceViewers[i].GetSliderValue());
            SellResource(resourceType, numberOfItems);
        }
    }
    #endregion

    #region Piracy
    void StealResource(ResourceType resourceType, int numberOfItemsToSteal, bool showButtonsAndSliders)
    {
        PlayerData playerData = HexGridController.player.PlayerData;
        activeShipInventory.GetResource(resourceType).Value -= numberOfItemsToSteal; //Take from boarded ship
        playerData.Resources.GetResource(resourceType).Value += numberOfItemsToSteal; //Give to player
        UpdateUI(resourceType, showButtonsAndSliders);
    }

    //Button call
    public void StealMarkedResources()
    {
        for (int i = 0; i < resourceViewers.Length; i++)
        {
            ResourceType resourceType = (ResourceType)i;
            int numberOfItems = Mathf.RoundToInt(resourceViewers[i].GetSliderValue());
            StealResource(resourceType, numberOfItems, true);
        }
    }

    public void StealAllResources()
    {
        for (int i = 0; (ResourceType)i != ResourceType.MAX; i++)
        {
            StealResource((ResourceType)i, activeShipInventory.GetResource((ResourceType)i).Value, false);
        }
    }

    #endregion
}
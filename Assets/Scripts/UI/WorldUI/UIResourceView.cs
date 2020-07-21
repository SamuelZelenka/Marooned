using UnityEngine;
using UnityEngine.UI;

public class UIResourceView : MonoBehaviour
{
    [SerializeField] Text resourceNumber = null;
    ResourceType resourceType;
    private void OnDisable()
    {
        switch (resourceType)
        {
            case ResourceType.Wool:
                ShipData.Resource.OnWoolChanged -= UpdateUI;
                break;
            case ResourceType.Tobacco:
                ShipData.Resource.OnTobaccoChanged -= UpdateUI;
                break;
            case ResourceType.Coffee:
                ShipData.Resource.OnCoffeeChanged -= UpdateUI;
                break;
            case ResourceType.Silk:
                ShipData.Resource.OnSilkChanged -= UpdateUI;
                break;
            case ResourceType.Ores:
                ShipData.Resource.OnOresChanged -= UpdateUI;
                break;
            case ResourceType.MAX:
                break;
            default:
                break;
        }
    }
    public void Setup(ResourceType resourceType)
    {
        this.resourceType = resourceType;
        switch (resourceType)
        {
            case ResourceType.Wool:
                ShipData.Resource.OnWoolChanged += UpdateUI;
                break;
            case ResourceType.Tobacco:
                ShipData.Resource.OnTobaccoChanged += UpdateUI;
                break;
            case ResourceType.Coffee:
                ShipData.Resource.OnCoffeeChanged += UpdateUI;
                break;
            case ResourceType.Silk:
                ShipData.Resource.OnSilkChanged += UpdateUI;
                break;
            case ResourceType.Ores:
                ShipData.Resource.OnOresChanged += UpdateUI;
                break;
            case ResourceType.MAX:
                break;
            default:
                break;
        }
        UpdateUI();
    }
    public void UpdateUI()
    {
        resourceNumber.text = HexGridController.player.PlayerData.ShipData.GetResource(resourceType).Value.ToString();
    }
}

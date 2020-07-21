using UnityEngine;
using UnityEngine.UI;

public class UIResourceView : MonoBehaviour
{
    [SerializeField] Text resourceNumber = null;
    [SerializeField] ResourceType resourceType;
    private void OnDisable()
    {
        if (HexGridController.player != null)
        {
            HexGridController.player.PlayerData.ShipData.GetResource(resourceType).OnResourceChanged -= UpdateUI;
        }
    }

    private void OnEnable()
    {
        if (HexGridController.player != null)
        {
            ShipData.Resource resource = HexGridController.player.PlayerData.ShipData.GetResource(resourceType);
            resource.OnResourceChanged += UpdateUI;
            UpdateUI(resource);
        }
    }
    
    void UpdateUI(ShipData.Resource resource)
    {
        resourceNumber.text = resource.Value.ToString();
    }
}

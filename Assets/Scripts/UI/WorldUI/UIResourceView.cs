using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UIResourceView : MonoBehaviour
{
    [SerializeField] Text resourceNumber = null;
    [SerializeField] ResourceType resourceType = ResourceType.MAX;
    private void OnDisable()
    {
        if (HexGridController.player != null)
        {
            HexGridController.player.PlayerData.Resources.GetResource(resourceType).OnResourceChanged -= UpdateUI;
        }
    }

    private void OnEnable()
    {
        if (HexGridController.player != null)
        {
            Resource resource = HexGridController.player.PlayerData.Resources.GetResource(resourceType);
            resource.OnResourceChanged += UpdateUI;
            UpdateUI(resource);
        }
    }

    void UpdateUI(Resource resource)
    {
        resourceNumber.text = resource.Value.ToString();
    }
}

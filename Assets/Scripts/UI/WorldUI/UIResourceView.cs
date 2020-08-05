using UnityEngine;
using UnityEngine.UI;

public class UIResourceView : MonoBehaviour
{
    [SerializeField] Text resourceNumber = null;
    [SerializeField] ResourceType resourceType = ResourceType.MAX;

    Player player;

    private void OnDisable()
    {
        if (player != null)
        {
            player.PlayerData.Resources.GetResource(resourceType).OnResourceChanged -= UpdateUI;
        }
    }

    private void OnEnable()
    {
        if (player != null)
        {
            Setup(player);
        }
        else
        {
            SessionSetup.OnPlayerCreated += Setup;
        }
    }
    void Setup(Player player)
    {
        this.player = player;
        Resource resource = player.PlayerData.Resources.GetResource(resourceType);
        resource.OnResourceChanged += UpdateUI;
        UpdateUI(resource);
    }
    void UpdateUI(Resource resource)
    {
        resourceNumber.text = resource.Value.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIResourceController : MonoBehaviour
{
    [SerializeField] UIResourceView[] resources = null;

    private void OnEnable()
    {
        if (HexGridController.player != null)
        {
            for (int i = 0; i < resources.Length; i++)
            {
                ShipData playerShipData = HexGridController.player.PlayerData.ShipData;
                resources[i].Setup((ResourceType)i);
            }
        }
    }

    void UpdateUI(ResourceType resourceType)
    {
        resources[(int)resourceType].UpdateUI();
    }
}

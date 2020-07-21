using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipInspectView : MonoBehaviour
{
    [SerializeField] ResourceView[] resources = null;
    [SerializeField] GameObject stealAllButtonObject = null;
    [SerializeField] Text stealAllButtonText = null;

    public void Setup(ShipData shipData, bool interactable)
    {
        stealAllButtonObject.SetActive(interactable);

        int totalValue = 0;
        for (int i = 0; i < resources.Length; i++)
        {
            int defaultValue = 0;
            ResourceType resourceType = (ResourceType) i;
            switch (resourceType)
            {
                case ResourceType.Wool:
                    defaultValue = ShipData.WOOLDEFAULTVALUE;
                    break;
                case ResourceType.Tobacco:
                    defaultValue = ShipData.TOBACCODEFAULTVALUE;
                    break;
                case ResourceType.Coffee:
                    defaultValue = ShipData.COFFEEDEFAULTVALUE;
                    break;
                case ResourceType.Silk:
                    defaultValue = ShipData.SILKDEFAULTVALUE;
                    break;
                case ResourceType.Ores:
                    defaultValue = ShipData.ORESDEFAULTVALUE;
                    break;
                case ResourceType.MAX:
                    break;
            }

            ShipData.Resource resource = shipData.GetResource(resourceType);
            int numberOfUnits = resource.Value;
            totalValue += numberOfUnits * defaultValue;

            resources[i].Setup(resource, interactable, defaultValue);
        }
        stealAllButtonText.text = $"Steal All (£{totalValue.ToString()})";
    }
}

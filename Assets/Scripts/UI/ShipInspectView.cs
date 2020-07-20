using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInspectView : MonoBehaviour
{
    [SerializeField] ResourceView[] resources = null;

    public void Setup(ShipData shipData)
    {
        for (int i = 0; i < resources.Length; i++)
        {
            resources[i].Setup(shipData.GetResource((ResourceType)i), true);
        }
    }
}

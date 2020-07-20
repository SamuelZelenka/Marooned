using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantController : MonoBehaviour
{
    [SerializeField] MerchantResource[] resourceObjects;
    Harbor myHarbor;

    void UpdateUI(/* INSERT RESOURCE ENUM */)
    {
        for (int i = 0; i < resourceObjects.Length; i++)
        {
            resourceObjects[i].UpdateUI(myHarbor.merchantData);
        }
    }
    void SellResource(/* INSERT RESOURCE ENUM */)
    {
        UpdateUI();
    }
    void SellAll(/* INSERT RESOURCE ENUM */)
    {
        SellResource(/* INSERT RESOURCE ENUM */);
    }
}
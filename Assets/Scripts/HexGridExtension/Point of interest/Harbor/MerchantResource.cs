using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MerchantResource : MonoBehaviour
{
    public Image resourceImage;
    public Slider amountSlider;
    public Button sellButton;
    public Button sellAllButton;

    public void UpdateUI(/* INSERT RESOURCE ENUM */Harbor.MerchantData merchantData)
    {
        /*
        switch (INSERT RESOURCE ENUM )
        {
            case Resources.wool:
                break;
            case Resources.tobacco:
                break;
            case Resources.coffee:
                break;
            case Resources.silk:
                break;
            case Resources.ore:
                break;
            default:
                break;
        }
        */
    }

}

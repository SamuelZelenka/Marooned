using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Simon Voss
//Holder of information of which to display to a tooltip

public class TooltipInformation : MonoBehaviour
{
    [SerializeField] [TextArea(1, 3)] string information = "";

    public void SetInformation(string newInformation) => information = newInformation;

    public void SendToTooltip() => MouseTooltip.SetUpToolTip(MouseTooltip.ColorText.Default, information);

    public void DisableTooltip() => MouseTooltip.HideTooltip();
}

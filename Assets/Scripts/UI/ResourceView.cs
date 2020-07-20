using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceView : MonoBehaviour
{
    [SerializeField] Text interactNumber = null;
    [SerializeField] Slider slider = null;
    [SerializeField] Button interactableActionButton = null;

    public void Setup(ShipData.Resource resource, bool showButtonsAndSliders)
    {
        interactNumber.text = "0";
        if (showButtonsAndSliders)
        {
            slider.maxValue = resource.value;
            slider.value = 0;
            slider.interactable = resource.value > 0;
            interactableActionButton.interactable = false;
        }
    }

    public float GetSliderValue() => slider.value;

    public void ChangeSelectedValue(float dynamicFloat)
    {
        int number = Mathf.RoundToInt(dynamicFloat);
        slider.value = number;
        interactNumber.text = number.ToString();
        interactableActionButton.interactable = number > 0;
    }
}

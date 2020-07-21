using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceView : MonoBehaviour
{
    [SerializeField] Text interactNumber = null;
    [SerializeField] Slider slider = null;
    [SerializeField] Button interactableActionButton = null;
    int valuePerItem;
    [SerializeField] Text valuePerItemText = null;
    [SerializeField] string interactableButtonMainText = "Sell";
    [SerializeField] Text interactableButtonText = null;


    public void Setup(ShipData.Resource resource, bool showButtonsAndSliders, int valuePerItem)
    {
        interactNumber.text = "0";
        this.valuePerItem = valuePerItem;
        this.valuePerItemText.text = "£" + valuePerItem.ToString();

        if (showButtonsAndSliders)
        {
            slider.maxValue = resource.Value;
            slider.interactable = resource.Value > 0;
            ChangeSelectedValue(0f);
            slider.value = 0f;
        }
    }

    public float GetSliderValue() => slider.value;

    public void ChangeSelectedValue(float dynamicFloat)
    {
        int number = Mathf.RoundToInt(dynamicFloat);
        interactNumber.text = number.ToString();
        interactableActionButton.interactable = number > 0;
        interactableButtonText.text = interactableButtonMainText + " (£" + (valuePerItem * number).ToString() + ")";
    }
}

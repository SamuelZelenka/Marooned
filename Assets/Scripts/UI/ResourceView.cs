using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceView : MonoBehaviour
{
    [SerializeField] Text numberText = null;
    [SerializeField] Slider slider = null;
    int valuePerItem;
    [SerializeField] Text valuePerItemText = null;

    public delegate void ResourceViewHandler();
    public static ResourceViewHandler OnSliderValueChanged;

    public void Setup(ShipData.Resource resource, bool showButtonsAndSliders, int valuePerItem)
    {
        numberText.text = showButtonsAndSliders ? "0" : resource.Value.ToString();
        this.valuePerItem = valuePerItem;
        this.valuePerItemText.text = "£" + valuePerItem.ToString();

        slider.gameObject.SetActive(showButtonsAndSliders);
        if (showButtonsAndSliders)
        {
            slider.maxValue = resource.Value;
            slider.interactable = resource.Value > 0;
            ChangeSelectedValue(0f);
            slider.value = 0f;
        }
    }

    public void ChangeSliderToMax() => slider.value = slider.maxValue;

    public float GetSliderValue() => slider.value;

    public void ChangeSelectedValue(float dynamicFloat)
    {
        int number = Mathf.RoundToInt(dynamicFloat);
        numberText.text = number.ToString();
        OnSliderValueChanged?.Invoke();
    }
}

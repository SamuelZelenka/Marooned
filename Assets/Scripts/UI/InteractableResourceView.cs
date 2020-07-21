using UnityEngine;
using UnityEngine.UI;

public class InteractableResourceView : MonoBehaviour
{
    [SerializeField] Text numberText = null;
    [SerializeField] Slider slider = null;
    int valuePerItem;
    int maxNumber;
    [SerializeField] Text valuePerItemText = null;
    [SerializeField] ResourceType resourceType = ResourceType.MAX; 

    public delegate void ResourceViewHandler();
    public static ResourceViewHandler OnSliderValueChanged;

    public void Setup(Resource resource, bool showButtonsAndSliders, int valuePerItem)
    {
        maxNumber = resource.Value;
        numberText.text = showButtonsAndSliders ? $"0 / {maxNumber}" : resource.Value.ToString();
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

    public ResourceType ResourceType { get => resourceType; }

    public void ChangeSliderToMax() => slider.value = slider.maxValue;

    public float GetSliderValue() => slider.value;

    public void ChangeSelectedValue(float dynamicFloat)
    {
        int number = Mathf.RoundToInt(dynamicFloat);
        numberText.text = $"{number.ToString()} / {maxNumber}";
        OnSliderValueChanged?.Invoke();
    }
}

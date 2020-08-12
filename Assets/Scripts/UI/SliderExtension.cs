using UnityEngine;
using UnityEngine.UI;

public class SliderExtension : MonoBehaviour
{
    [SerializeField] Slider slider = null;
    [SerializeField] float floatChange = 0.01f;
    [SerializeField] Text value = null;

    private void Start()
    {
        PrintValue();
    }

    public void ValueUp()
    {
        if (slider.wholeNumbers)
            slider.value++;
        else
            slider.value += floatChange;
    }
    public void ValueDown()
    {
        if (slider.wholeNumbers)
            slider.value--;
        else
            slider.value -= floatChange;
    }

    public void PrintValue() => value.text = slider.wholeNumbers ? slider.value.ToString() : Utility.FactorToPercentageText(slider.value);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] Slider slider = null;
    [SerializeField] Text currentValueText = null;

    [SerializeField] float animationTime = 1f;

    //Use this method to set the max value of the slider
    public void SetMaxValue(int value)
    {
        if (value > 0)
        {
            slider.maxValue = value;
        }
        else
        {
            Debug.LogWarning(value + " is not over 0!");
        }
    }

    //Use this one to set the fill of the bar 
    public void SetCurrentValue(int newValue)
    {
        if (this.gameObject.activeInHierarchy)
        {
            StartCoroutine(ChangeSliderValueOverTime(slider, newValue));
        }
        else
        {
            Debug.LogWarning("Trying to call on an disabled bar");
            Debug.LogWarning("This was called on" + gameObject.transform + "/" + gameObject.transform.parent + "/" + gameObject.transform.parent.parent);
        }
    }

    IEnumerator ChangeSliderValueOverTime(Slider slider, int targetValue)
    {
        float timer = 0;
        float t = 0;
        int sliderStartValue = Mathf.RoundToInt(slider.value);

        while (timer < this.animationTime)
        {
            timer += Time.deltaTime;
            t = timer / animationTime;
            slider.value = Mathf.Lerp(sliderStartValue, targetValue, t);
            ShowValueInText(slider.value);
            yield return null;
        }
        slider.value = targetValue;

        ShowValueInText(slider.value);
    }

    private void ShowValueInText(float currentValue)
    {
        if (currentValueText != null)
        {
            currentValueText.text = Mathf.RoundToInt(currentValue).ToString() + " / " + Mathf.RoundToInt(slider.maxValue).ToString();
        }
    }
}

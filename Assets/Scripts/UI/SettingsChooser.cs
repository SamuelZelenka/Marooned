using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsChooser : MonoBehaviour
{
    public delegate void IndexHandler(int newIndex);
    public IndexHandler OnIndexChanged;

    public Text choice;
    int currentIndex;
    List<string> options;

    private void SetText() => choice.text = options[currentIndex];

    public void AddOption(string option)
    {
        if (options == null)
        {
            options = new List<string>();
            options.Add(option);
            SetText();
        }
        else
        {
            options.Add(option);
        }
    }

    public void Next()
    {
        currentIndex++;
        if (currentIndex >= options.Count)
            currentIndex = 0;
        SetText();
        OnIndexChanged?.Invoke(currentIndex);
    }
    public void Previous()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = options.Count - 1;
        SetText();
        OnIndexChanged?.Invoke(currentIndex);
    }
}

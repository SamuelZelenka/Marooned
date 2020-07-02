using UnityEngine;
using UnityEngine.UI;

public class MouseHoverImage : TooltipInformation
{
    [SerializeField] Image image = null;
    public void UpdateUI(string text, Sprite sprite)
    {
        image.sprite = sprite;
        SetInformation(text);
    }
}

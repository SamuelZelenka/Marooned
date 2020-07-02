using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatLogMessage : MonoBehaviour
{
    [SerializeField] Image image = null;
    [SerializeField] TooltipInformation tooltip = null;
    public Sprite portrait = null; 
    public string log = "";
    public void UpdateUI()
    {
        image.sprite = portrait;
        tooltip.SetInformation(log);
    }

}

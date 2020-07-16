using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUIView : MonoBehaviour
{
    public GameObject harborPanel;
    public Text harborTitleText;

    public void ShowHarbor(PointOfInterest pointOfInterest)
    {
        harborPanel.SetActive(true);
        harborTitleText.text = pointOfInterest.name;
    }
}

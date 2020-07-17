using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUIView : MonoBehaviour
{
    public GameObject poiPanel;
    public Text poiTextTitle;
    public GameObject openPOIButton;

    private PointOfInterest latestPOI;

    private void OnEnable()
    {
        HexUnit.OnUnitBeganMove += DisablePOIButton;
    }

    private void OnDisable()
    {
        HexUnit.OnUnitBeganMove -= DisablePOIButton;
    }

    public void EnablePOIButton(PointOfInterest pointOfInterest)
    {
        openPOIButton.SetActive(true);
        latestPOI = pointOfInterest;
    }

    public void DisablePOIButton(HexUnit unitMoved)
    {
        if (unitMoved.playerControlled)
        {
            openPOIButton.SetActive(false);
        }
    }

    public void OpenPOI()
    {
        poiPanel.SetActive(true);
        poiTextTitle.text = latestPOI.name;
    }
}

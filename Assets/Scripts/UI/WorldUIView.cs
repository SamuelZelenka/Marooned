using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUIView : MonoBehaviour
{
    

    private void OnEnable()
    {
        HexUnit.OnUnitBeganMove += DisablePOIInteraction;
        HexUnit.OnUnitBeganMove += DisableCannonInteraction;
        HexUnit.OnUnitBeganMove += DisableBoardingInteraction;
    }

    private void OnDisable()
    {
        HexUnit.OnUnitBeganMove -= DisablePOIInteraction;
        HexUnit.OnUnitBeganMove -= DisableCannonInteraction;
        HexUnit.OnUnitBeganMove -= DisableBoardingInteraction;
    }

    #region POI
    private PointOfInterest latestPOI;
    public GameObject poiPanel;
    public Text poiTextTitle;
    public GameObject openPOIButton;
    public void EnablePOIInteraction(PointOfInterest pointOfInterest)
    {
        openPOIButton.SetActive(true);
        latestPOI = pointOfInterest;
    }

    private void DisablePOIInteraction(HexUnit unitMoved)
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
    #endregion

    #region ShipToShip
    public GameObject shootCannonButton;
    public GameObject boardShipButton;

    public void EnableBoardingInteraction()
    {
        boardShipButton.SetActive(true);
    }

    private void DisableBoardingInteraction(HexUnit unitMoved)
    {
        boardShipButton.SetActive(false);
    }

    public void EnableCannonInteraction()
    {
        shootCannonButton.SetActive(true);
    }

    private void DisableCannonInteraction(HexUnit unitMoved)
    {
        shootCannonButton.SetActive(false);
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUIView : MonoBehaviour
{
    [SerializeField] GameObject poiPanel = null;
    [SerializeField] HarborView harborView = null;
    [SerializeField] GameObject openPOIButton = null;
    [SerializeField] GameObject shipInspectPanel = null;
    [SerializeField] ShipInspectView shipInspectView = null;
    [SerializeField] BoardingController boardingController = null;

    private void OnEnable()
    {
        HexUnit.OnUnitBeganMove += DisablePOIInteraction;
        HexUnit.OnUnitBeganMove += CloseBoardingView;
    }

    private void OnDisable()
    {
        HexUnit.OnUnitBeganMove -= DisablePOIInteraction;
        HexUnit.OnUnitBeganMove -= CloseBoardingView;
    }

    #region POI
    PointOfInterest latestPOI;
    public void EnablePOIInteraction(PointOfInterest pointOfInterest)
    {
        openPOIButton.SetActive(true);
        latestPOI = pointOfInterest;
    }

    private void DisablePOIInteraction(HexUnit unitMoved)
    {
        openPOIButton.SetActive(false);
    }

    public void OpenPOI()
    {
        poiPanel.SetActive(true);
        switch (latestPOI.MyType)
        {
            case PointOfInterest.Type.Harbor:
                harborView.Setup(latestPOI as Harbor);
                break;
        }
    }
    #endregion

    public void OpenBoardingView(Ship boardedShip, Ship boardedByShip)
    {
        shipInspectPanel.SetActive(true);
        //Display Resources for the boarded ship
        shipInspectView.Setup(boardedShip.ShipData, true);
        boardingController.Setup(boardedShip.ShipData);
    }

    private void CloseBoardingView(HexUnit unitMoved)
    {
        shipInspectPanel.SetActive(false);
    }
}

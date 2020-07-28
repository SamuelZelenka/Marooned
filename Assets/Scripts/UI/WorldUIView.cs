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
    [SerializeField] ResourceInteractionController shipInspectController = null;

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

    public void OpenBoardingView(Player boardedPlayer, Player boardedByPlayer)
    {
        shipInspectPanel.SetActive(true);
        //Display Resources for the boarded ship
        shipInspectController.Setup(boardedPlayer.PlayerData.Resources, true);
    }

    public void OpenInspectView(Player inspectedPlayer, Player inspectingPlayer)
    {
        shipInspectPanel.SetActive(true);
        shipInspectController.Setup(inspectedPlayer.PlayerData.Resources, false);
    }

    private void CloseBoardingView(HexUnit unitMoved)
    {
        shipInspectPanel.SetActive(false);
    }
}

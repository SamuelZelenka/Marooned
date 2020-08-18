using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUIView : MonoBehaviour
{
    [SerializeField] GameObject poiPanel = null;
    [SerializeField] GameObject harborPanel = null;
    [SerializeField] GameObject strongholdPanel = null;
    [SerializeField] HarborView harborView = null;
    [SerializeField] StrongholdView strongholdView = null;
    [SerializeField] GameObject openPOIButton = null;
    [SerializeField] GameObject shipInspectPanel = null;
    [SerializeField] ResourceInteractionController shipInspectController = null;

    private void OnEnable()
    {
        HexUnit.OnAnyUnitBeganMove += DisablePOIInteraction;
        HexUnit.OnAnyUnitBeganMove += CloseBoardingView;
    }

    private void OnDisable()
    {
        HexUnit.OnAnyUnitBeganMove -= DisablePOIInteraction;
        HexUnit.OnAnyUnitBeganMove -= CloseBoardingView;
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
        harborPanel.SetActive(false);
        strongholdPanel.SetActive(false);

        latestPOI.InteractedWith();
        switch (latestPOI.MyType)
        {
            case PointOfInterest.Type.Harbor:
                harborPanel.SetActive(true);
                harborView.Setup(latestPOI as Harbor);
                break;
            case PointOfInterest.Type.Stronghold:
                strongholdPanel.SetActive(true);
                strongholdView.Setup(latestPOI as Stronghold);
                break;
        }
    }
    #endregion

    public void OpenBoardingView(Player boardedPlayer, Player boardedByPlayer)
    {
        shipInspectPanel.SetActive(true);
        //Display Resources for the boarded ship
        shipInspectController.Setup(boardedPlayer.PlayerData.Resources, true, false);
    }

    public void OpenInspectView(Player inspectedPlayer, Player inspectingPlayer)
    {
        shipInspectPanel.SetActive(true);
        shipInspectController.Setup(inspectedPlayer.PlayerData.Resources, false, false);
    }

    private void CloseBoardingView(HexUnit unitMoved)
    {
        shipInspectPanel.SetActive(false);
    }
}

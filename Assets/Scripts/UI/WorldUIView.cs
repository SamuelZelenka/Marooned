using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUIView : MonoBehaviour
{
    [SerializeField] GameObject poiPanel = null;
    [SerializeField] Text poiTextTitle = null;
    [SerializeField] GameObject openPOIButton = null;
    [SerializeField] GameObject boardingPanel = null;

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

    public void OpenBoardingView() => boardingPanel.SetActive(true);

    private void CloseBoardingView(HexUnit unitMoved)
    {
        if (unitMoved.playerControlled)
        {
            boardingPanel.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipViewer : MonoBehaviour
{
    [SerializeField] GameObject viewButton = null;
    [SerializeField] GameObject shootButton = null;
    [SerializeField] GameObject boardingButton = null;

    public void ShowViewPossible(bool status)
    {
        viewButton.SetActive(status);
    }
    public void ShowShootingPossible(bool status)
    {
        shootButton.SetActive(status);
    }
    public void ShowBoardingPossible(bool status)
    {
        boardingButton.SetActive(status);
    }
}

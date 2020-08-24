using UnityEngine;

public class ShipViewer : MonoBehaviour
{
    [SerializeField] GameObject viewButton = null;
    [SerializeField] GameObject shootButton = null;
    [SerializeField] GameObject boardingButton = null;

    private void Awake()
    {
        HexUnit.OnAnyUnitBeganMove += HideAll;
    }

    private void HideAll(HexUnit unit)
    {
        ShowViewPossible(false);
        ShowShootingPossible(false);
        ShowBoardingPossible(false);
    }

    public void ShowViewPossible(bool status)
    {
        if (viewButton)
            viewButton.SetActive(status);
    }
    public void ShowShootingPossible(bool status)
    {
        if (shootButton)
            shootButton.SetActive(status);
    }
    public void ShowBoardingPossible(bool status)
    {
        if (boardingButton)
            boardingButton.SetActive(status);
    }
}

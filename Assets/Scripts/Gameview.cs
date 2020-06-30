using UnityEngine;

public class Gameview : MonoBehaviour
{
    [SerializeField] GameObject combatAndManagementView = null;
    [SerializeField] GameObject combatCanvas = null;

    [SerializeField] GameObject mapView = null;


    private void OnEnable()
    {
        HexGridController.OnModeChangedTo += ModeChanged;
    }

    private void OnDisable()
    {
        HexGridController.OnModeChangedTo -= ModeChanged;
    }

    private void ModeChanged(HexGridController.GridMode newMode)
    {
        switch (newMode)
        {
            case HexGridController.GridMode.Map:
                StartMapView();
                break;
            case HexGridController.GridMode.Combat:
                StartCombatView();
                break;
            case HexGridController.GridMode.Management:
                StartCrewManagementView();
                break;
        }
    }

    void StartCrewManagementView()
    {
        combatAndManagementView.SetActive(true);
        mapView.SetActive(false);
        combatCanvas.SetActive(false);
    }
     void StartMapView()
    {
        mapView.SetActive(true);
        combatAndManagementView.SetActive(false);
        combatCanvas.SetActive(false);
    }
    void StartCombatView()
    {
        combatAndManagementView.SetActive(true);
        combatCanvas.SetActive(true);
        mapView.SetActive(false);
    }
}

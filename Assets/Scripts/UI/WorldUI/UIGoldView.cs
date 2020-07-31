using UnityEngine;
using UnityEngine.UI;

public class UIGoldView : MonoBehaviour
{
    [SerializeField] Text gold = null;

    void Setup()
    {
        HexGridController.player.PlayerData.OnGoldChanged += UpdateUI;
        UpdateUI();
    }

    private void OnEnable()
    {
        if (HexGridController.player != null)
        {
            HexGridController.player.PlayerData.OnGoldChanged += UpdateUI;
            UpdateUI();
        }
        else
        {
            SessionSetup.OnPlayerCreated += Setup;
        }
    }

    private void OnDisable()
    {
        if (HexGridController.player != null)
        {
            HexGridController.player.PlayerData.OnGoldChanged -= UpdateUI;
        }
    }

    void UpdateUI()
    {
        gold.text = HexGridController.player.PlayerData.Gold.ToString();
    }
}

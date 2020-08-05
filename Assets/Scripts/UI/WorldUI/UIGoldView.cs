using UnityEngine;
using UnityEngine.UI;

public class UIGoldView : MonoBehaviour
{
    [SerializeField] Text gold = null;

    Player player;
    void Setup(Player player)
    {
        this.player = player;
        player.PlayerData.OnGoldChanged += UpdateUI;
        UpdateUI();
    }

    private void OnEnable()
    {
        if (player != null)
        {
            player.PlayerData.OnGoldChanged += UpdateUI;
            UpdateUI();
        }
        else
        {
            SessionSetup.OnPlayerCreated += Setup;
        }
    }

    private void OnDisable()
    {
        if (player != null)
        {
            player.PlayerData.OnGoldChanged -= UpdateUI;
        }
    }

    void UpdateUI()
    {
        gold.text = player.PlayerData.Gold.ToString();
    }
}

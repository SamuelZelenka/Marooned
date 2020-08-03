using UnityEngine;

public class BountyView : MonoBehaviour
{
    [SerializeField] ProgressBar bountyProgress = null;


    void Setup(PlayerData playerdata)
    {
        playerdata.OnBountyChanged += UpdateUI;
        UpdateUI();
    }

    private void OnEnable()
    {
        if (HexGridController.player != null)
        {
            HexGridController.player.PlayerData.OnBountyChanged += UpdateUI;
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
            HexGridController.player.PlayerData.OnBountyChanged -= UpdateUI;
        }
    }

    void UpdateUI()
    {
        bountyProgress.EnqueueChange(new ProgressBar.ProgressStatus(HexGridController.player.PlayerData.Bounty, HexGridController.player.PlayerData.NextBountyChange, HexGridController.player.PlayerData.PreviousBountyChange));
    }
}
using UnityEngine;

public class BountyView : MonoBehaviour
{
    [SerializeField] Bar bountyProgress = null;
    Player player;

    void Setup(Player player)
    {
        this.player = player;
        player.PlayerData.OnBountyChanged += UpdateUI;
        UpdateUI();
    }

    private void OnEnable()
    {
        if (player != null)
        {
            player.PlayerData.OnBountyChanged += UpdateUI;
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
            player.PlayerData.OnBountyChanged -= UpdateUI;
        }
    }

    void UpdateUI()
    {
        bountyProgress.EnqueueChange(new Bar.ProgressStatus(player.PlayerData.Bounty, player.PlayerData.NextBountyChange, player.PlayerData.PreviousBountyChange));
    }
}
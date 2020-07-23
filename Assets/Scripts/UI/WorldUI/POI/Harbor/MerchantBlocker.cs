using UnityEngine;

public class MerchantBlocker : MonoBehaviour
{
    private void OnEnable()
    {
        if (HexGridController.player == null)
        {
            gameObject.SetActive(false);
            return;
        }
        if (HexGridController.player.PlayerData.BountyLevel.CurrentValue == Bounty.MAXBOUNTYLEVEL)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
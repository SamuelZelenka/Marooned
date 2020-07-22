using UnityEngine;

public class MerchantBlocker : MonoBehaviour
{
    private void OnEnable()
    {
        if (HexGridController.player.PlayerData.BountyLevel == PlayerData.MAXBOUNTYLEVEL)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
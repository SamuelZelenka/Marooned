using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBountyLevelView : MonoBehaviour
{
    [SerializeField] Bar bountyLevel;

    private void OnDisable()
    {
        if (HexGridController.player != null)
        {
            HexGridController.player.PlayerData.OnBountyChanged -= UpdateUI;
        }
    }

    private void OnEnable()
    {
        
        if (HexGridController.player != null)
        {
            HexGridController.player.PlayerData.OnBountyChanged += UpdateUI;
            bountyLevel.SetMaxValue(PlayerData.MAXBOUNTYLEVEL);
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        bountyLevel.SetCurrentValue(HexGridController.player.PlayerData.BountyLevel);
    }
}

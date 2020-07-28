﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGoldView : MonoBehaviour
{
    [SerializeField] Text gold = null;
    private void OnDisable()
    {
        if (HexGridController.player != null)
        {
            HexGridController.player.PlayerData.OnGoldChanged -= UpdateUI;
        }
    }

    private void OnEnable()
    {
        if (HexGridController.player != null)
        {
            HexGridController.player.PlayerData.OnGoldChanged += UpdateUI;
            UpdateUI();
        }
    }
    void UpdateUI()
    {
        gold.text = HexGridController.player.PlayerData.Gold.ToString();
    }
}
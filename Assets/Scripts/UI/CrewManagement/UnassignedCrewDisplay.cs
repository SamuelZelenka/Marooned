using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnassignedCrewDisplay : MouseHoverImage
{
    public Character character;
    public void OnClick()
    {
        HexGridController.SelectedCell = character.Location;
        Debug.Log($"{HexGridController.player.Crew.Count} Crew members");
    }
}

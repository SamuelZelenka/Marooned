using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPortrait : MonoBehaviour
{
    const float DOUBLECLICKTIME = 0.5f;

    public Image profileImage;
    Character character;

    bool isClicked = false;
    float doubleClickTimer;
    public void OnClick()
    {
        if (!isClicked)
        {
            if (HexGridController.SelectedCell != character && character != null)
            {
                HexGridController.SelectedCell = character.Location;
            }
            doubleClickTimer = DOUBLECLICKTIME;
            isClicked = true;
        }
        else
        {
            HexGridController.SelectedCell = character.Location;
        }
    }
    public void UpdatePortrait(Character character)
    {
        this.character = character;
        profileImage.sprite = character.characterData.portrait;
    }
    private void Update()
    {
        if (isClicked)
        {
            if (doubleClickTimer < 0)
            {
                isClicked = false;
            }
            else
            {
                doubleClickTimer -= Time.deltaTime;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobPosition : MonoBehaviour
{
    public PlayerInput input;
    public CrewSimulation.ShipJob job;
    public CrewSimulation crewSimulation;
    public Image portrait;
    public Sprite defaultPortrait = null;


    public void RemoveCharacter()
    {
        crewSimulation.SetCharacterJob(null, job);
        portrait.sprite = defaultPortrait;
    }

    public void SetCharacter()
    {
        Character selectedCharacter = input.GetSelectedCharacter();
        if (selectedCharacter)
        {
            crewSimulation.SetCharacterJob(selectedCharacter, job);
            portrait.sprite = selectedCharacter.portrait;
        }
    }
}

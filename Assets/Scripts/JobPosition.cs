using UnityEngine;
using UnityEngine.UI;

public class JobPosition : MonoBehaviour
{
    public CrewSimulation.ShipJob job;
    [SerializeField] CrewSimulation crewSimulation = null;
    [SerializeField] Image portrait = null;
    [SerializeField] Sprite defaultPortrait = null;

    public Character CharacterOnJob { get; private set; }

    [SerializeField] CrewJobDisplay crewJobDisplay = null;

    public bool HasCharacter
    {
        get => CharacterOnJob != null;
    }

    public void RemoveCharacter()
    {
        CharacterOnJob = null;
        portrait.color = new Color(1,1,1,0);
        portrait.sprite = defaultPortrait;
    }

    public void SetCharacterToJob(Character newCharacter)
    {
        crewJobDisplay.character = newCharacter;
        CharacterOnJob = newCharacter;
        portrait.color = new Color(1, 1, 1, 1);
        portrait.sprite = newCharacter.portrait;
    }

    //Input from the UI-system when a character is placed on a job
    public void ClickDetected()
    {
        Character selectedCharacter = HexGridController.SelectedCharacter;
        if (!CharacterOnJob)
        {
            if (selectedCharacter && selectedCharacter.characterData.ShipJob == CrewSimulation.ShipJob.None)
            {
                crewSimulation.SetCharacterJob(selectedCharacter, job);
            }
        }
        else
        {
            crewSimulation.RemoveCharacterFromItsJob(CharacterOnJob, job);
        }
    }
}

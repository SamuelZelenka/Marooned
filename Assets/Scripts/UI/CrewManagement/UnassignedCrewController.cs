using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnassignedCrewController : MonoBehaviour
{

    [SerializeField] CrewJobDisplay unassignedCharacterPrefab = null;
    [SerializeField] Transform unassignedCharacterParent = null;

    List<Character> unassignedCharacters = new List<Character>();
    List<CrewJobDisplay> unassignedCharacterImages = new List<CrewJobDisplay>();

    public void UpdateUnassignedCharacterList()
    {
        unassignedCharacters.Clear();
        foreach (Character character in HexGridController.player.Crew)
        {
            if (character.characterData.ShipJob == CrewSimulation.ShipJob.None)
            {
                unassignedCharacters.Add(character);
            }
        }

        if (SyncCharacterList())
        {
            for (int i = 0; i < unassignedCharacters.Count; i++)
            {
                string effectDescription = unassignedCharacters[i].characterData.characterName;
                Sprite characterPortrait = unassignedCharacters[i].portrait;
                unassignedCharacterImages[i].character = unassignedCharacters[i];
                unassignedCharacterImages[i].UpdateUI(effectDescription, characterPortrait);
            }
        }

        bool SyncCharacterList()
        {
            if (unassignedCharacterImages.Count < unassignedCharacters.Count)
            {
                while (unassignedCharacterImages.Count < unassignedCharacters.Count)
                {
                    unassignedCharacterImages.Add(Instantiate(unassignedCharacterPrefab, unassignedCharacterParent.transform));
                }
            }
            if (unassignedCharacterImages.Count > unassignedCharacters.Count)
            {
                while (unassignedCharacterImages.Count > unassignedCharacters.Count)
                {
                    Destroy(unassignedCharacterImages[0].gameObject);
                    unassignedCharacterImages.RemoveAt(0);
                }
            }
            return unassignedCharacterImages.Count == unassignedCharacters.Count;
        }
    }
}

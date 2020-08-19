using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewDisplay : MonoBehaviour
{
    List<CharacterView> characterPortraits = new List<CharacterView>();
    [SerializeField] CharacterView prefab = null;

    public void UpdateCrew(List<Character> crew)
    {

        if (characterPortraits.Count < crew.Count) // too many
        {
            while (characterPortraits.Count < crew.Count)
            {
                characterPortraits.Add(Instantiate(prefab, transform));
            }
        }
        if (characterPortraits.Count > crew.Count) // too few
        {
            while (characterPortraits.Count > crew.Count)
            {
                Destroy(characterPortraits[0].gameObject);
                characterPortraits.RemoveAt(0);
            }
        }

        for (int i = 0; i < characterPortraits.Count ; i++)
        {
            characterPortraits[i].SetCharacter(crew[i]);
        }
    }
}

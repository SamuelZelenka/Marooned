using System.Collections.Generic;
using UnityEngine;
public class PartyDisplay : MonoBehaviour
{
    List<PartyMember> partyMembers = null;

    public void UpdateParty(List<Character> playerCrew)
    {
        for (int i = 0; i < playerCrew.Count; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            partyMembers[i].UpdateUI(playerCrew[i]);
            if (transform.childCount > playerCrew.Count)
            {
                for (int j = playerCrew.Count; j < transform.childCount; j++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }
}
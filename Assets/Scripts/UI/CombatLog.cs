using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CombatLog : MonoBehaviour
{
    [SerializeField] int combatLogLimit = 6;
    [SerializeField] MouseHoverImage prefab = null;
    Queue<MouseHoverImage> combatLog = new Queue<MouseHoverImage>();
    private void OnEnable()
    {
        CombatTurnSystem.OnTurnEnding += NewLog;
    }
    private void OnDisable()
    {
        CombatTurnSystem.OnTurnEnding -= NewLog;

    }
    public void NewLog(Character character)
    {
        MouseHoverImage message = Instantiate(prefab, transform);

        if (combatLog.Count > combatLogLimit)
        {
            DestroyImmediate(combatLog.Dequeue().gameObject);
        }
        combatLog.Enqueue(message);
        message.UpdateUI(character.logMessage.Message, character.characterData.portrait);
    }
}

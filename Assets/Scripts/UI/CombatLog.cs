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
        string text;
        MouseHoverImage logBox = Instantiate(prefab, transform);

        if (combatLog.Count > combatLogLimit)
        {
            DestroyImmediate(combatLog.Dequeue().gameObject);
        }
        combatLog.Enqueue(logBox);
        if (character.logMessage.Message == "")
        {
            text = "Did nothing this turn.";
        }
        else
        {
            text = character.logMessage.Message;
        }
        logBox.UpdateUI(text, character.characterData.portrait);
    }
}

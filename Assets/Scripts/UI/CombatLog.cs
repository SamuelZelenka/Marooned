using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CombatLog : MonoBehaviour
{
    [SerializeField] int combatLogLimit = 6;
    [SerializeField] CombatLogMessage prefab = null;
    Queue<CombatLogMessage> combatLog = new Queue<CombatLogMessage>();

    
    public void NewLog(string logMessage, Character attacker)
    {
        CombatLogMessage message = Instantiate(prefab, transform);
        message.log = logMessage;
        message.portrait = attacker.characterData.portrait;

        if (combatLog.Count > combatLogLimit)
        {
            DestroyImmediate(combatLog.Dequeue().gameObject);
        }
        combatLog.Enqueue(message);
        message.UpdateUI();
    }
}

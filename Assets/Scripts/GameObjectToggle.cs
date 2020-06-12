using System.Collections.Generic;
using UnityEngine;

public class GameObjectToggle : MonoBehaviour
{
    public List<GameObject> objectsToToggle = new List<GameObject>();

    public void Toggle()
    {
        foreach (var item in objectsToToggle)
        {
            item.SetActive(!item.activeSelf);
        }
    }
}

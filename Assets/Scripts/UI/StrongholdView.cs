using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrongholdView : MonoBehaviour
{
    [SerializeField] Text poiTextTitle = null;


    public void Setup(Stronghold stronghold)
    {
        poiTextTitle.text = stronghold.Name;
    }
}

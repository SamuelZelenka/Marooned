using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HarborView : MonoBehaviour
{
    [SerializeField] Text poiTextTitle = null;
    [SerializeField] ResourceInteractionController merchantController = null;

    public void Setup(Harbor harbor)
    {
        poiTextTitle.text = harbor.name;
        merchantController.Setup(harbor);
    }
}

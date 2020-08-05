using UnityEngine;
using UnityEngine.UI;

public class QuestLogElement : MonoBehaviour
{
    public RectTransform rect;
    [SerializeField] Text title = null;
    [SerializeField] Text description = null;

    public Quest Quest { private set; get; }

    public void Setup(Quest quest)
    {
        this.Quest = quest;
        this.title.text = quest.title;
        this.description.text = quest.description;
    }
}

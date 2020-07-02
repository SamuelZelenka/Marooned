using UnityEngine;
using UnityEngine.UI;

//Simon Voss
//Pop-up tooltip that shows information upon request from UI elements etc.

public class MouseTooltip : MonoBehaviour
{
    public enum ColorText { Default, Allowed, Forbidden }
    #region Singleton
    public static MouseTooltip instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Another instance of : " + instance.ToString() + " was tried to be instanced, but was destroyed from gameobject: " + this.transform.name);
            GameObject.Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    [Header("References")]
    [SerializeField] RectTransform canvasRect = null;
    [SerializeField] RectTransform mainRectTransform = null;
    [SerializeField] Text textField = null;
    [SerializeField] RectTransform backgroundRect = null;
    [SerializeField] Animator animator = null;

    [Header("Colors")]
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color allowed = Color.green;
    [SerializeField] Color forbidden = Color.red;

    float textPadding = 0;

    private void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, null, out Vector2 localPoint);
        transform.localPosition = localPoint;

        Vector2 anchoredPosition = mainRectTransform.anchoredPosition;
        if (anchoredPosition.x + backgroundRect.rect.width > canvasRect.rect.width)
        {
            anchoredPosition.x = canvasRect.rect.width - backgroundRect.rect.width;
        }
        if (anchoredPosition.y + backgroundRect.rect.height > canvasRect.rect.height)
        {
            anchoredPosition.y = canvasRect.rect.height - backgroundRect.rect.height;
        }
        mainRectTransform.anchoredPosition = anchoredPosition;
    }

    private void Start()
    {
        textPadding = textField.rectTransform.anchoredPosition.x;
        Hide();
    }

    private void SetUp(ColorText textColor, string message)
    {
        if (message == "")
        {
            HideTooltip();
            return;
        }

        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        animator.SetTrigger("FadeIn");

        Color color = defaultColor;
        switch (textColor)
        {
            case ColorText.Default:
                color = defaultColor;
                break;
            case ColorText.Allowed:
                color = allowed;
                break;
            case ColorText.Forbidden:
                color = forbidden;
                break;
        }

        textField.color = color;
        textField.text = message;

        Vector2 backgroundSize = new Vector2(textField.preferredWidth + textPadding * 2, textField.preferredHeight + textPadding * 2);
        backgroundRect.sizeDelta = backgroundSize;
    }

    private void Hide()
    {
        animator.SetTrigger("FadeOut");
        gameObject.SetActive(false);
    }

    public static void SetUpToolTip(ColorText textColor, string message) => instance.SetUp(textColor, message);

    public static void HideTooltip() => instance.Hide();
}

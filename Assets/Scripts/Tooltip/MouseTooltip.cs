using UnityEngine;
using UnityEngine.UI;

//Simon Voss
//Pop-up tooltip that shows information upon request from UI elements etc.

public class MouseTooltip : MonoBehaviour
{
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
            if (usedInCombatScene)
            {
               // CombatDelegates.instance.OnPreparedActionChanged += HideTooltip;
            }
        }
    }
    #endregion

    [SerializeField] bool usedInCombatScene = true;

    [SerializeField] Text textField = null;
    [SerializeField] RectTransform backgroundRect = null;

    [SerializeField] float textPadding = 10;


    [SerializeField] Animator animator = null;

    [Header("Colors")]
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color allowed = Color.green;
    [SerializeField] Color forbidden = Color.red;

    public enum ColorText { Default, Allowed, Forbidden }

    float mouseOffset = 25;

    private void Update()
    {
        Vector2 pos = Input.mousePosition;
        if (Input.mousePosition.x > Screen.width / 2)
        {
            pos += new Vector2(-mouseOffset - backgroundRect.sizeDelta.x, mouseOffset);
        }
        else
        {
            pos += new Vector2(mouseOffset, mouseOffset);
        }
        this.transform.position = pos;
    }

    private void Start()
    {
        HideTooltip(null);
    }


    private void SetUp(ColorText textColor, string message)
    {
        if (message == "")
        {
            HideTooltip();
            return;
        }


        gameObject.SetActive(true);
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

        Vector2 pos = Input.mousePosition;
        if (Input.mousePosition.x > Screen.width / 2)
        {
            pos += new Vector2(-mouseOffset - backgroundSize.x, mouseOffset);
        }
        else
        {
            pos += new Vector2(mouseOffset, mouseOffset);
        }
        this.transform.position = pos;
    }

    private void HideTooltip(Character character)
    {
        animator.SetTrigger("FadeOut");
        gameObject.SetActive(false);
    }

    public static void SetUpToolTip(ColorText textColor, string message)
    {
        instance.SetUp(textColor, message);
    }

    public static void HideTooltip()
    {
        instance.HideTooltip(null);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class UIWindDirection : MonoBehaviour
{
    [SerializeField] Sprite[] windSprites = null;
    Image image;
    private void Start()
    {
        image = GetComponent<Image>();
    }
    private void OnEnable()
    {
        OceanController.OnWindChanged += UpdateUI;
    }
    private void OnDisable()
    {
        OceanController.OnWindChanged -= UpdateUI;
    }
    public void UpdateUI(HexDirection direction, int windStrength)
    {
        switch (direction)
        {
            case HexDirection.NE:
                transform.rotation = Quaternion.Euler(0,0,315);
                break;
            case HexDirection.E:
                transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
            case HexDirection.SE:
                transform.rotation = Quaternion.Euler(0, 0, 225);
                break;
            case HexDirection.SW:
                transform.rotation = Quaternion.Euler(0, 0, 135);
                break;
            case HexDirection.W:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case HexDirection.NW:
                transform.rotation = Quaternion.Euler(0, 0, 25);
                break;
            default:
                break;
        }
        image.sprite = windSprites[windStrength];
    }
}
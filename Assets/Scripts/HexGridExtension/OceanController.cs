using UnityEngine;

public class OceanController : MonoBehaviour
{
    [SerializeField] MapTurnSystem mapTurnSystem = null;

    public static HexDirection WindDirection { get; private set; }
    public static WindStrength Wind { get; private set; }
    public enum WindStrength { Dead, Calm, Windy, Storm }
    public static float[] windSpeeds = new float[] { 0.01f, 0.1f, 0.3f, 0.6f };

    private void OnEnable()
    {
        mapTurnSystem.OnDayEnded += RandomizeWeather;
    }

    private void OnDisable()
    {
        mapTurnSystem.OnDayEnded -= RandomizeWeather;
    }

    private void Start() => RandomizeWeather();

    //Debug
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RandomizeWeather();
            }
        }
    }

    private void RandomizeWeather()
    {
        WindStrength windStrength = (WindStrength)Random.Range((int)WindStrength.Dead, (int)WindStrength.Storm + 1);
        HexDirection direction = HexDirectionExtension.ReturnRandomDirection();
        ChangeWindDirectionAndSpeed(direction, windStrength);
    }


    public static void ChangeWindDirectionAndSpeed(HexDirection newDirection, WindStrength newWind)
    {
        Material ocean = Resources.Load<Material>("Materials/Water");

        ocean.DisableKeyword("_WATERDIRECTION_NE");
        ocean.DisableKeyword("_WATERDIRECTION_E");
        ocean.DisableKeyword("_WATERDIRECTION_SE");
        ocean.DisableKeyword("_WATERDIRECTION_SW");
        ocean.DisableKeyword("_WATERDIRECTION_W");
        ocean.DisableKeyword("_WATERDIRECTION_NW");

        ocean.EnableKeyword($"_WATERDIRECTION_{newDirection.ToString()}");

        ocean.SetFloat("_Speed", windSpeeds[(int)newWind]);

        Wind = newWind;
        WindDirection = newDirection;

        Debug.Log($"Wind is now {newWind} in the {newDirection} direction");
    }
}

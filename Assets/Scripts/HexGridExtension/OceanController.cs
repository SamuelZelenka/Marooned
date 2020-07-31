using UnityEngine;

public class OceanController : MonoBehaviour
{
    [SerializeField] MapTurnSystem mapTurnSystem = null;

    enum WindStrength { Dead, Calm, Windy, Storm }
    static HexDirection windDirection;
    static WindStrength windStrength;
    static readonly float[] windSpeeds = new float[] { 0.01f, 0.1f, 0.3f, 0.6f };

    const int TAILWINDMOVEMENT = -1;
    const int SIDEWINDMOVEMENT = 0;
    const int HEADWINDMOVEMENT = 1;
    static readonly int[] windStengthFactor = new int[] { 4, 1, 2, 4};
    

    private void OnEnable()
    {
        mapTurnSystem.OnDayEnded += RandomizeWeather;
    }

    private void OnDisable()
    {
        mapTurnSystem.OnDayEnded -= RandomizeWeather;
    }

    private void Start() => RandomizeWeather();

    private void RandomizeWeather()
    {
        WindStrength windStrength = (WindStrength)Random.Range((int)WindStrength.Dead, (int)WindStrength.Storm + 1);
        HexDirection direction = HexDirectionExtension.ReturnRandomDirection();
        ChangeWindDirectionAndSpeed(direction, windStrength);
    }


    private void ChangeWindDirectionAndSpeed(HexDirection newDirection, WindStrength newWindStrength)
    {
        Material ocean = Resources.Load<Material>("Materials/Water");

        ocean.DisableKeyword("_WATERDIRECTION_NE");
        ocean.DisableKeyword("_WATERDIRECTION_E");
        ocean.DisableKeyword("_WATERDIRECTION_SE");
        ocean.DisableKeyword("_WATERDIRECTION_SW");
        ocean.DisableKeyword("_WATERDIRECTION_W");
        ocean.DisableKeyword("_WATERDIRECTION_NW");

        ocean.EnableKeyword($"_WATERDIRECTION_{newDirection.ToString()}");

        ocean.SetFloat("_Speed", windSpeeds[(int)newWindStrength]);

        windStrength = newWindStrength;
        windDirection = newDirection;

        Debug.Log($"Wind is now {newWindStrength} in the {newDirection} direction");
    }

    public static int GetOceanMovementModifier(HexDirection directionToMove)
    {
        if (windStrength == WindStrength.Dead)
        {
            return windStengthFactor[0];
        }
        int modifier;

        if (directionToMove == windDirection)
        {
            modifier = TAILWINDMOVEMENT * windStengthFactor[(int)windStrength];
        }
        else if (directionToMove == windDirection.Opposite())
        {
            modifier = HEADWINDMOVEMENT * windStengthFactor[(int)windStrength];
        }
        else
        {
            modifier = SIDEWINDMOVEMENT * windStengthFactor[(int)windStrength];
        }
        return modifier;
    }
}

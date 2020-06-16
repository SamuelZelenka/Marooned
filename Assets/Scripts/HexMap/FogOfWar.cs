using UnityEngine;
using UnityEngine.Tilemaps;

public class FogOfWar : MonoBehaviour
{
    Tilemap fogOfWar;

    private void Start()
    {
        //Subscribe Unit Moved To Ship
    }

    private void UnitMoved(HexUnit unit)
    {
        if (unit.playerControlled)
        {
            //UpdateFogOfWar(unit.Location.coordinates, unit.visionRange);
        }
    }

    public void UpdateFogOfWar(HexCoordinates unitPosition, int unitVisionRange)
    {

    }
}

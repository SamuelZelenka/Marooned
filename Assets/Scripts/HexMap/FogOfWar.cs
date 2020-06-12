using UnityEngine;
using UnityEngine.Tilemaps;

public class FogOfWar : MonoBehaviour
{
    int playerIndex = 0;
    Tilemap fogOfWar;

    private void Start()
    {
        
    }

    private void UnitMoved(HexUnit unit)
    {
        if (unit.playerIndex == playerIndex)
        {
            //UpdateFogOfWar(unit.Location.coordinates, unit.visionRange);
        }
    }

    public void UpdateFogOfWar(HexCoordinates unitPosition, int unitVisionRange)
    {

    }
}

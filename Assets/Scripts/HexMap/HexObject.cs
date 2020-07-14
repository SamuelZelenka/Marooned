using UnityEngine;

public class HexObject : MonoBehaviour
{
    HexCell location;
    public HexCell Location
    {
        get => location;
        set
        {
            if (location)
            {
                location.Unit = null;
            }
            location = value;
            value.Object = this;
            transform.localPosition = value.Position;
        }
    }
}

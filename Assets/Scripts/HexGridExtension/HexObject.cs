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
                location.Object = null;
            }
            location = value;
            value.Object = this;
            transform.localPosition = value.Position;
        }
    }

    public virtual void Despawn()
    {
        GameObject.Destroy(this.gameObject);
    }

    public void OnDestroy()
    {
        location.Object = null;
    }
}

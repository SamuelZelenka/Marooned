public abstract class PointOfInterest
{
    public delegate void PointOfInterestHandler(PointOfInterest pointOfInterest);
    public event PointOfInterestHandler OnPlayableUnitArrived;
    public event PointOfInterestHandler OnInteractedWith;
    public event PointOfInterestHandler OnIsKnownChanged;


    public string Name { get; private set; }
    bool isKnown;
    public bool IsKnown {
        get { return isKnown; }
        set {
            isKnown = value;
            OnIsKnownChanged?.Invoke(this); 
        }
    }

    public HexCell Hexcell { private set; get; }

    public PointOfInterest(string name, HexCell hexCell, PointOfInterestHandler pointOfInterestHandler, Type type)
    {
        this.Name = name;
        IsKnown = false;
        OnPlayableUnitArrived += pointOfInterestHandler;
        MyType = type;
        Hexcell = hexCell;
    }

    public void PlayableUnitArrived()
    {
        IsKnown = true;
        OnPlayableUnitArrived?.Invoke(this);
    }
    public void InteractedWith() => OnInteractedWith?.Invoke(this);

    public enum Type { Harbor, Stronghold }
    public Type MyType { get; private set; }
}
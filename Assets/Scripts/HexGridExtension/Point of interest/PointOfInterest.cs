public abstract class PointOfInterest
{
    public delegate void PointOfInterestHandler(PointOfInterest pointOfInterest);
    public event PointOfInterestHandler OnPlayableUnitArrived;
    public event PointOfInterestHandler OnInteractedWith;

    public string Name { get; private set; }
    public bool isKnown;

    public HexCell Hexcell { private set; get; }

    public PointOfInterest(string name, HexCell hexCell, PointOfInterestHandler pointOfInterestHandler, Type type)
    {
        this.Name = name;
        isKnown = false;
        OnPlayableUnitArrived += pointOfInterestHandler;
        MyType = type;
        Hexcell = hexCell;
    }

    public void PlayableUnitArrived()
    {
        isKnown = true;
        OnPlayableUnitArrived?.Invoke(this);
    }
    public void InteractedWith() => OnInteractedWith?.Invoke(this);

    public enum Type { Harbor, Stronghold }
    public Type MyType { get; private set; }
}
public abstract class PointOfInterest
{
    public delegate void PointOfInterestHandler(PointOfInterest pointOfInterest);
    public event PointOfInterestHandler OnPlayableUnitArrived;
    public event PointOfInterestHandler OnInteractedWith;

    public string Name { get; private set; }
    public HexCell Hexcell { private set; get; }

    public PointOfInterest(string name, HexCell hexCell, PointOfInterestHandler pointOfInterestHandler, Type type)
    {
        this.Name = name;
        OnPlayableUnitArrived += pointOfInterestHandler;
        MyType = type;
        Hexcell = hexCell;
    }

    public void PlayableUnitArrived() => OnPlayableUnitArrived?.Invoke(this);
    public void InteractedWith() => OnInteractedWith?.Invoke(this);

    public enum Type { Harbor, Stronghold }
    public Type MyType { get; private set; }
}
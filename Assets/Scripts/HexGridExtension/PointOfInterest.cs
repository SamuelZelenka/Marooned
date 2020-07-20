public abstract class PointOfInterest
{
    public delegate void PointOfInterestHandler(PointOfInterest pointOfInterest);
    public event PointOfInterestHandler OnInteractedWith;

    public string name;

    public PointOfInterest(string name, PointOfInterestHandler pointOfInterestHandler)
    {
        this.name = name;
        OnInteractedWith = pointOfInterestHandler;
    }

    public void InteractWith() => OnInteractedWith?.Invoke(this);
}

public class Harbor : PointOfInterest
{
    public Harbor(string name, PointOfInterestHandler pointOfInterestHandler) : base(name, pointOfInterestHandler)
    {

    }
}
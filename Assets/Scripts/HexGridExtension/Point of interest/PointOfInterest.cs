using System;

public abstract class PointOfInterest
{
    public delegate void PointOfInterestHandler(PointOfInterest pointOfInterest);
    public event PointOfInterestHandler OnInteractedWith;

    public string name;

    public PointOfInterest(string name, PointOfInterestHandler pointOfInterestHandler, Type type)
    {
        this.name = name;
        OnInteractedWith = pointOfInterestHandler;
        MyType = type;
    }

    public void InteractWith() => OnInteractedWith?.Invoke(this);

    public enum Type { Harbor}
    public Type MyType { get; private set; }
}
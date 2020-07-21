[System.Serializable]
public class Resource
{
    public delegate void ResourceHandler(Resource resource);
    public ResourceHandler OnResourceChanged;
    string name;
    int value;
    public int Value
    {
        get { return value; }
        set
        {
            this.value = value;
            OnResourceChanged?.Invoke(this);
        }
    }
    public Resource(string name, int value)
    {
        this.name = name;
        this.value = value;
    }

    public override string ToString()
    {
        return name;
    }
}

public class Cell
{
    private Resource _resource;
    private int _count = 0;

    public Resource Resource => _resource;

    public int Count => _count;
    public bool OverFlow => _count >= Ñonstants.MaxItemCount;

    public void Set(Resource resource)
    {
        _resource = resource;
        _count++;
    }

    public void Add()
        => _count++;

    public void Reset()
    {
        _resource = null;
        _count = 0;
    }

}
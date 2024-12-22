using UnityEditor.Experimental.GraphView;

public struct InventoryCell
{
    private ResourceCountPair _resources;

    private int _capacity;

    public Resource Resource => _resources.Resource;
    public int Capacity => _capacity;
    public int Count => _resources.Count;
    public bool OverFlow => _resources.Count >= _capacity;

    public InventoryCell(int capacity, Resource resource = null, int count = 0)
    {
        _resources = new(resource, count);
        _capacity = capacity;
    }

    public void SetResource(Resource resource)
        => _resources.SetResource(resource);

    public int Add(int count)
    {
        int currentCapacity = _capacity - Count;
        if (count <= currentCapacity)
            _resources.Count += count;
        else
        {
            _resources.Count += currentCapacity;
            return count - currentCapacity;
        }

        return 0;
    }


    public void Clear()
        => _resources.SetResource(null);
}
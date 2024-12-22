using UnityEngine;
[System.Serializable]
public struct ResourceCountPair
{
    [SerializeField]
    public Resource Resource { get; private set; }

    [field: SerializeField, Min(0)]
    public int Count { get; set; }

    public ResourceCountPair(Resource resource, int count)
    {
        Resource = resource;
        Count = count;
    }

    public void SetResource(Resource resource, int count = 0)
    {
        Resource = resource;
        Count = count;
    }
}
using System.Text.Json;
using System.Text.Json.Serialization;
using UnityEngine;
[System.Serializable]
public struct ResourceCountPair
{
    [field: SerializeField]
    public Resource Resource { get; private set; }

    [field: SerializeField, Min(0)]
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    public static ResourceCountPair FromJson(string source)
    {
        var pair = JsonSerializer.Deserialize<ResourceCountPair>(source);
        pair.Resource = Resource.FromString(pair.Name);

        return pair;
    }

    public ResourceCountPair(Resource resource, int count)
    {
        Resource = resource;
        Count = count;
        Name = resource.ResourceName;
    }

    public void SetResource(Resource resource, int count = 0)
    {
        Resource = resource;
        Count = count;
    }

    public override string ToString()
        => $"{Resource.ResourceName} : {Count}";
}
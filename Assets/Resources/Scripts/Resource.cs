using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewResource", menuName = "Resources", order = 51)]
public class Resource : ScriptableObject
{
    private static readonly Dictionary<string, Resource> _stringResourcePairs =
        Resources.LoadAll<Resource>("Prefabs").ToDictionary(r => r.ResourceName, r => r);

    private static readonly IEnumerable<string> _names = 
        Resources.LoadAll<Resource>("Prefabs").Select(r => r.ResourceName);

    [field: SerializeField]
    public string ResourceName { get; private set; }

    [field: SerializeField]
    public Sprite Sprite { get; private set; }

    public static IEnumerable<string> ResourceNames => _names;

    public static Resource FromString(string name)
    {
        if (_stringResourcePairs.TryGetValue(name, out Resource resource))
            return resource;

        throw new System.Exception($"Resource with name {name} didn't find");
    }
}

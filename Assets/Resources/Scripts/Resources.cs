using System.Collections.Generic;
using System.Linq;

namespace NTO24
{
    public static class Resources
    {
        private static Dictionary<string, Resource> _stringResourcePairs;

        private static IEnumerable<string> _names;

        public static IEnumerable<string> ResourceNames => _names;

        public static void Initialize()
        {
            _stringResourcePairs = UnityEngine.Resources
                .LoadAll<Resource>("Prefabs").ToDictionary(r => r.ResourceName, r => r);

            _names = UnityEngine.Resources
                .LoadAll<Resource>("Prefabs").Select(r => r.ResourceName);
        }

        public static Resource FromString(string name)
        {
            if (_stringResourcePairs.TryGetValue(name, out Resource resource))
                return resource;

            throw new System.Exception($"Resource with name {name} didn't find");
        }
    }
}
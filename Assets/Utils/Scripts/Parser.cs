using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NTO24
{
    public static class Parser
    {
        private static readonly StatInfo[] _stats;

        private static Dictionary<string, Resource> _stringResourcePairs;

        private static IEnumerable<string> _names;

        public static IEnumerable<string> ResourceNames => _names;

        static Parser()
        {
            _stats = Resources.LoadAll<StatInfo>("Stats");

            var resources = Resources.LoadAll<Resource>("Resources\\Source");

            _stringResourcePairs = resources.ToDictionary(r => r.ResourceName, r => r);

            _names = resources.Select(r => r.ResourceName);
        }

        public static StatInfo ToStat(this StatNames type)
            => _stats.FirstOrDefault(s => s.Type == type);

        public static Resource ToResource(this string str)
            => _stringResourcePairs[str];

        public static Pair<Resource, int> ToResources(this string str)
        {
            string[] pair = str.Split(":");

            Resource resource = _stringResourcePairs[pair[0]];
            int count = int.Parse(pair[1]);

            return new(resource, count);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace NTO24
{
    public class BearInfo
    {
        public Texture2D Sprite { get; private set; }

        public Texture2D Icon { get; private set; }

        public string Name { get; private set; }

        public List<EntityStat> Stats { get; private set; }

        public static List<BearInfo> bearInfos = new();

        public static BearInfo[] Bears = new BearInfo[3];

        private BearInfo(Texture2D texture, Texture2D icon, string name, string data)
        {
            Sprite = texture;

            Name = name;

            Icon = icon;

            var bearStats = new List<EntityStat>();

            var stats = JsonConvert.DeserializeObject<Dictionary<string, float[]>>(data);

            foreach (var pair in stats)
            {
                bearStats.Add(new(pair.Key.ToStatInfo(), pair.Value));
            }

            Stats = bearStats;
        }

        public static void Add(Texture2D texture, Texture2D icon, string name, string data)
        {
            if (bearInfos.Any(i => i.Name == name))
                return;
            var info = new BearInfo(texture, icon, name, data);
            bearInfos.Add(info);
        }
    }
}
using System.Linq;

namespace NTO24
{
    public static class JsonParser
    {
        private static readonly StatInfo[] _stats;

        static JsonParser()
        {
            _stats = UnityEngine.Resources.LoadAll<StatInfo>("Stats");
        }

        public static StatInfo ToStat(this StatsNames type)
            => _stats.FirstOrDefault(s => s.Type == type);

        /*public static EntityStatsType ToStat(this string str)
            => _stats.FirstOrDefault(s => s.Name == str);*/

        public static Resource ToResource(this string str)
        {
            return null;
        }
    }
}
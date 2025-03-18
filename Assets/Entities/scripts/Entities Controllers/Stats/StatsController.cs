using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class StatsController : EntityComponent, ISavableComponent
    {
        [SerializeField]
        private List<EntityStat> _stats;

        public UnityEvent OnDataChangeEvent { get; private set; } = new();

        public IEnumerable<EntityStat> Stats => _stats;

        public string Name => "Stats";

        public string[] Data
        {
            get
            {
                string[] data = new string[_stats.Count];

                for (int i = 0; i < data.Length; i++)
                    data[i] = new Pair<StatInfo, int>(_stats[i].StatInfo, _stats[i].CurrentLevel).ToString();

                return data;
            }
        }

        public EntityStat this[StatNames statsType]
        {
            get
            {
                var stat = _stats.FirstOrDefault(s => s.StatInfo == statsType.ToStatInfo());
                return stat != null ? stat : throw new StatMissedException(Entity as IStatsable, statsType);
            }
        }

        public void ServerInitialize(IEnumerable<string> data)
        {
            foreach (var stat in _stats)
                stat.OnUpgradeEvent.AddListener(OnDataChangeEvent.Invoke);

            for (int i = 0; i < _stats.Count; i++)
            {
                var pair = data.ElementAt(i).ToStats();

                this[pair.Value1.Type].CurrentLevel = pair.Value2;
            }
        }
    }
}
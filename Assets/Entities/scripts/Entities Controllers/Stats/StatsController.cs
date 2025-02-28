using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NTO24
{
    public class StatsController : EntityComponent
    {
        [SerializeField]
        private List<EntityStat> _stats;

        public IEnumerable<EntityStat> Stats => _stats;

        public EntityStat this[EntityStatsType statsType]
            => _stats.FirstOrDefault(s => s.Stat == statsType);
    }
}
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

        public EntityStat this[StatsNames statsType]
            => _stats.FirstOrDefault(s => s.StatInfo == statsType.ToStat());
    }
}
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

        public EntityStat this[StatNames statsType]
        {
            get
            {
                var stat = _stats.FirstOrDefault(s => s.StatInfo == statsType.ToStat());
                return stat != null ? stat : throw new StatMissedException(Entity as IStatsable, statsType);
            }
        }
    }
}
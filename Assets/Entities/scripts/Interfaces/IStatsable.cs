using System.Collections.Generic;
using UnityEngine;

namespace NTO24
{
    public interface IStatsable : IEntity
    {
        public StatsController StatsController { get; }

        public IEnumerable<EntityStat> Stats => StatsController.Stats;

        public EntityStat this[EntityStatsType statsType]
            => StatsController[statsType];

    }
}